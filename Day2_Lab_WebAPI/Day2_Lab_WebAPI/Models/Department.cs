using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Day2_Lab_WebAPI.Models
{
    public class Department
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50,MinimumLength = 2)]
        [Display(Name = "Department Name")] 
        public string? Name { get; set; }

        [JsonIgnore] // to solve the serlixation problem
        public virtual List<Student> Students { get; set; } = new List<Student>();
    }
}
