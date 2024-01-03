using System.ComponentModel.DataAnnotations;

namespace DB_Assigment.DTOs
{
    public class ProductDto
    {
        [Required]
        [MinLength(3)]
        public string? Code { get; set; }
        [Required]
        [MinLength(3)]
        public string? Name { get; set; }
        [Required]
        public decimal? Price { get; set; }
        [Required]
        public float? Discount { get; set; }
        [Required]
        public int? MinQuantity { get; set; }
        [Required]
        public int? CategoryId { get; set; }
        [Required]
        public string? ImagePath { get; set; }
    }
}
