
using System.ComponentModel.DataAnnotations;

namespace PlantNests.Models
{
    public class Paymenttype
    {
        [Key]
        public int paymenttypeId { get; set; }
        public string PaymentType { get; set; }

    }
}

