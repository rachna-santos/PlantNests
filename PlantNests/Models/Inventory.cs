using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantNests.Models
{
    public class Inventory
    {
        [Key]
        public int inventoryId { get; set; }
        public int productId { get; set; }
        public virtual Product Product { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime Lastmodifield { get; set; }
    }
}
