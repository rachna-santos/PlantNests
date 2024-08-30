using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantNests.Models
{
    public class Inventory
    {
        [Key]
        public int inventoryId { get; set; }

        [ForeignKey("Customer")]
        public int Id { get; set; }
        public virtual Customer Customer { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public int productId { get; set; }
        public virtual Product Product { get; set; }
        public int quantity { get; set; }
        public int Inqty { get; set; }
        public int totalqty { get; set; }
        public decimal totalamount { get; set; }
 
        public DateTime CreateDate { get; set; }
        public DateTime Lastmodifield { get; set; }
    }
}
