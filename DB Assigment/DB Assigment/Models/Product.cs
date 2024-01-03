using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB_Assigment.Models
{
    public class Product
    {
        [MinLength(3)]
        [Required]
        public string Code { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        public string? ImageName { get; set; }
        [Required]
        public decimal Price { get; set; }
        public int? MinQuantity { get; set; }
        public float? DiscoundRate { get; set; }
        public Category Category { get; set; }
    }
}
