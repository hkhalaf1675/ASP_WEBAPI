using System.ComponentModel.DataAnnotations;

namespace DB_Assigment.Models
{
    public class Category
    {
        public int Id { get; set; }
        [MinLength(2)]
        [Required]
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
