using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PlantNests.Models
{
    public class ProductModel
    {
        [Key]
        public int productId { get; set; }
        [Required]
        [MaxLength(200)]
        public string productName { get; set; }
        [Required]
        [MaxLength(200)]
        public string description { get; set; }
        [Required]
        public int price { get; set; }
        public int categoryId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime Lastmodifield { get; set; }

  
    }
}
