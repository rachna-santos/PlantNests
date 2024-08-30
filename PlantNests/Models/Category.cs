using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PlantNests.Models
{
    public class Category
    {
        [Key]
        public int categoryId { get; set; }
        [Required]
        [MaxLength(200)]
        public string categoryName { get; set; }
        [Required]
        [MaxLength(200)]
        public string description { get; set; }
        [Required]
        public int price { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime Lastmodifield { get; set; }
        public string? imagepath { get; set; }
        [NotMapped]
        public IFormFile profilepicture { get; set; }

    }
}
