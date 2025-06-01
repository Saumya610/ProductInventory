using System.ComponentModel.DataAnnotations;
using RangeAttribute = System.ComponentModel.DataAnnotations.RangeAttribute;

namespace WebApp_Assessment.Source.DTOs
{
    public class ProductUpdateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "StockAvailable must be non-negative")]
        public int StockAvailable { get; set; }

        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Price must be non-negative")]
        public decimal Price { get; set; }
    }
}
