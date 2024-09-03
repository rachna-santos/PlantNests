using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantNests.Models
{
    public class Payment
    {
        [Key]
        public int paymentId { get; set; }
        public int paymenttypeId { get; set; }
        public Paymenttype paymenttype { get; set; }    
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [ForeignKey("Customer")]
        public int Id { get; set; }
        public virtual Customer Customer {get; set;}
        public int TotalAmount {get; set;}
        public string PaymentStatus { get; set;}
        public DateTime CreateDate { get; set; }
        public DateTime Lastmodifield { get; set; }
    }
}
