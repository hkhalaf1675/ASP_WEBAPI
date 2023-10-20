using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Day2_Lab_WebAPI.Models
{
    public class Student
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50,MinimumLength = 3)]
        public string? Name { get; set; }
        [Range(14,50)]
        public int? Age { get; set; }
        [Range(1,4)]
        public int? Level { get; set; }
        [ForeignKey("Department")]
        public int? DeptID { get; set; }

        // the serlization problem :
        // --> because the loop that any student has department 
        // ------ and any department has list of students
        // ---> on c# it's not a problem
        // ------> but the problem when convert it into json 
        // to solve that there is :
        // 1-> the attribute of [JsonIgnore] thet prevent converting the property to json
        // ---> on the list of students on department class and that will solved
        // ------ it because will not convert the list but convert department naivational
        // 2-> the second soluation of serilation is creating DTO(Data Transfer Object)
        // ---> DTO is like ViewModel on MVC : to carry spacific data from one table or more
        // ------ DTO should not have complex data type only primative datatype
        // ------- because the complex type have the same problem of serilzation
        public virtual Department Department { get; set; }
    }
}
