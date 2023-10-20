using System.ComponentModel.DataAnnotations;

namespace FullStackTask.Models
{
    public class Category
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50,MinimumLength = 2)]
        public string? Name { get; set; }

        public virtual List<Product> Products { get; set; } = new List<Product>();
    }
}
