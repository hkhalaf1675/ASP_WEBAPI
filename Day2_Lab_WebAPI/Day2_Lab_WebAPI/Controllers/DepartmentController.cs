using Day2_Lab_WebAPI.Contexts;
using Day2_Lab_WebAPI.DTO;
using Day2_Lab_WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Day2_Lab_WebAPI.Controllers
{
    [Route("api/[controller]")]// 
    [ApiController]// to make complier know that controller is api controller not mvc conroller 
    public class DepartmentController : ControllerBase
    {
        private readonly ITIDbContext context;

        public DepartmentController(ITIDbContext _context)
        {
            context = _context;
        }
        // each action should before it the attribute of type of verb by HttpVerb
        // [HttpPut] there is edit on all of fields
        // [HttpPatch] there is edit on some of fields
        // can also detect the verb by the name of action :
        // ----------->> should starts of ends with varb : e.g : GetAll() , PostDepartment
        // to write or create more actio  with the same verb :
        // determine the route of one of them or all of them to make difference 
        // -> [Route("{id}")] : that will take id an input param : url will be :-> /api/Department/{,1,2,3,..}
        // [Route("All")] : the url will be :-> /api/Department/All
        // the route should not start with / : to concat the route name with the main route of controller
        // and there is Overload on attribute HttpVerb takes the route : e.g : [HttpGet("All")]

        // the responsible of binding on web api is the attribute of [ApiController] above the controller
        // any primtive type by default will take it from URL[Route Segment] ,or Query string, or will put the default on it
        // any complex type(class or struct) be defualt will taked from Body

        // [FromRoute] : url segment
        // [FromQuery] : url query string
        // [FromBody] : body {from body}
        // [FromServices] : from injections

        // on the one action can use the same attribute only one time
        // --> but can use more than one attribute

        // CROS Policy
        // -- happen when external api request that api { not the same localhost}
        // 1-> Customize the policy , declare the configration method
        // ----> add service : AddCors() -> on it -> AddPolicy()
        // --- can take the orgin that can use the request APIs : can take one or list
        // .WithOrgins("http://localhost:4200","http://localhost:1230")
        // -- or can make it to any one can use it
        // .AllowAnyOrgin().AllowAnyHeader().AllowAnyMethod()
        // -> any IP ------  any header ----- any request {get,post,..}
        // 2-> add the middleware :-> app.UseCors("nameOfPolicy");


        //[HttpGet]
        //[Route("All")]
        //// instead of the above two
        [HttpGet("All")]
        public IActionResult GetAll()
        {
            List<Department> list = context.Departments.ToList();
            if (list.Count > 0)
                return Ok(list);
            return NotFound();
        }
        [HttpGet("{id}",Name = "GetDepartmentByID")]
        // the name of route : the names of route on the same controller must be unique
        // it returns the url of that request
        // -> http://localhost123456/api/Department/{id}
        public IActionResult GetByID(int id)
        {
            Department dept = context.Departments.FirstOrDefault(D => D.ID == id);
            if (dept != null)
                return Ok(dept);
            return NotFound();
        }
        [HttpGet("{name:alpha}")]
        public IActionResult GetByName(string name)
        {
            Department dept = context.Departments.FirstOrDefault(D => D.Name.ToLower() == name.ToLower());
            if (dept != null)
                return Ok(dept);
            return NotFound();
        }

        [HttpPost]
        public IActionResult PostDepartment(Department department)
        {
            if (ModelState.IsValid)
            {
                if (CheckDepartmentExists(department.ID))
                    return Conflict("There is Department already exists with that ID");
                context.Departments.Add(department);
                try
                {
                    context.SaveChanges();
                    return Created(Url.Link("GetDepartmentByID", new { id = department.ID }), department);
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id:int}")]
        public IActionResult PutDepartment([FromRoute]int id,[FromBody]Department newDept)
        {
            if (ModelState.IsValid)
            {
                Department oldDept = context.Departments.FirstOrDefault(D => D.ID == id);
                if(oldDept != null)
                {
                    oldDept.Name = newDept.Name;
                    try
                    {
                        context.SaveChanges();
                        return StatusCode(204, "Saved");
                    }
                    catch(Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
                return NotFound("There is no Department with that ID");
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteDepartment(int id)
        {
            Department dept = context.Departments.Find(id);
            if (dept != null)
            {
                try
                {
                    context.Departments.Remove(dept);
                    context.SaveChanges();
                    return StatusCode(204, "Department Removed");
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return NotFound("there is no department with that id");
        }
        [HttpGet("DepartmentStudents/{id:int}")]
        public IActionResult GetDeptStudents(int id)
        {
            Department dept = context.Departments.Include(D => D.Students).FirstOrDefault(D => D.ID == id);
            if(dept == null)
            {
                return NotFound();
            }
            else
            {
                DepartmentDataWithListOfStudentDTO department = new DepartmentDataWithListOfStudentDTO();
                department.ID = id;
                department.DepartmentName = dept.Name;
                foreach(Student std in dept.Students)
                {
                    department.StudentName.Add(std.Name);
                }
                return Ok(department);
            }
        }

        private bool CheckDepartmentExists(int id)
        {
            return context.Departments.Where(D => D.ID == id).ToList().Count > 0;
        }
    }
}
