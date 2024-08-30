
using System.ComponentModel.DataAnnotations;

namespace PlantNests.Models
{
    public class SoldProduct
    {
        [Key]
        public int soldid { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int productId { get; set; }
        public virtual Product Product { get; set; }
        public int quantitysold { get; set; }
        public int TotalAmount  { get; set; }
        public DateTime solddate { get; set; }

    }
}
