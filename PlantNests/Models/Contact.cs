using PlantNests.Migrations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantNests.Models
{
    public class Contact
    {
        [Key]
        public int contactId { get; set; }

        [ForeignKey("Customer")]
        public int Id { get; set; }
        public virtual Customer Customer { get; set; }
        public string Message { get; set; }
    }
}
