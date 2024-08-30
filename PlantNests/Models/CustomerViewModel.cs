using System.ComponentModel.DataAnnotations;

namespace PlantNests.Models
{
    public class CustomerViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Number { get; set; }
        public string address { get; set; }
    }
}
