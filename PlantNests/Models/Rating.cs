using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantNests.Models
{
    public class Rating
    {
        [Key]
        public int ReviewId { get; set; }
        public int review { get; set; }
        public string? comment { get; set; }
        public int productId { get; set; }
        public virtual Product Product { get; set; }
        [ForeignKey("Customer")]
        public int Id { get; set; }
        public virtual Customer Customer { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime Lastmodifield { get; set; }


    }
}
