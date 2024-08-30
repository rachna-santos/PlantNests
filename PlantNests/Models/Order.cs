using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PlantNests.Models
{
    public class Order
    {

        [Key]
        public int OrderId { get; set; }
        public string ordernumber { get; set; }
        public int productId { get; set; }
        public virtual Product Product { get; set; }
        public int quantity { get; set; }
        [ForeignKey("Customer")]
        public int Id { get; set; }
        public virtual Customer Customer { get; set; }
        public int subtotal { get; set; }
        public int totalamount { get; set; }
        public string Status { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime CreateDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Lastmodifield { get; set; }
    }
}
