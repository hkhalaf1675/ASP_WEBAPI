using System.ComponentModel.DataAnnotations;

namespace FullStackTask.DTO
{
    public class ProductDataDTO
    {
        public int? Code { get; set; }
        public string? ProductName { get; set; }
        public string? ImageName { get; set; }
        public decimal? Price { get; set; }
        public int? MinQuantity { get; set; }
        public int? DiscountRate { get; set; }
        public string? CategoryName { get; set; }
    }
}
