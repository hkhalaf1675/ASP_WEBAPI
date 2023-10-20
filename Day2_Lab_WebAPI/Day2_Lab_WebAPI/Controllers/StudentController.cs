using Day2_Lab_WebAPI.Contexts;
using Day2_Lab_WebAPI.DTO;
using Day2_Lab_WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Day2_Lab_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ITIDbContext context;

        public StudentController(ITIDbContext _context)
        {
            context = _context;
        }
        [HttpGet("All")]
        public IActionResult GetAll()
        {
            List<StudentDataWithDepartmentNameDTO> students = new List<StudentDataWithDepartmentNameDTO>();
            foreach (Student std in context.Students.Include(S => S.Department).ToList())
            {
                students.Add(new StudentDataWithDepartmentNameDTO()
                {
                    ID = std.ID,
                    StudentName = std.Name,
                    Level = std.Level,
                    DepartmentName = std.Department.Name
                });
            }
            if (students.Count > 0)
                return Ok(students);
            else
                return NotFound();
        }
        [HttpGet("{id:int}")]
        public IActionResult GetStudentByID(int id)
        {
            Student std = context.Students.Include(S => S.Department).FirstOrDefault(S => S.ID == id);
            if(std== null)
                return NotFound();
            else
            {
                StudentDataWithDepartmentNameDTO student = new StudentDataWithDepartmentNameDTO()
                {
                    ID = std.ID,
                    StudentName = std.Name,
                    Level = std.Level,
                    DepartmentName = std.Department.Name
                };
                return Ok(student);
            }
        }
    }
}
