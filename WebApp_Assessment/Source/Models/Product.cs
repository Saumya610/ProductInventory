using System.ComponentModel.DataAnnotations;

namespace WebApp_Assessment.Source.Models
{
    public class Product
    {
        [Key]
        public string ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int StockAvailable { get; set; }

        public decimal Price { get; set; }
    }
}
