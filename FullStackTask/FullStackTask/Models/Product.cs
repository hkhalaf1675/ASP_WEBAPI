using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullStackTask.Models
{
    public class Product
    {
        [Key]
        public int? Code { get; set; }
        [Required]
        [StringLength(100,MinimumLength = 2)]
        public string? Name { get; set; }
        public string? ImageName { get; set; }
        public decimal? Price { get; set; }
        public int? MinQuantity { get; set; }
        [Range(0,100)]
        public int? DiscountRate { get; set; }
        [ForeignKey("Category")]
        public int? CategoryID { get; set; }
        public virtual Category? Category { get; set; }
    }
}
