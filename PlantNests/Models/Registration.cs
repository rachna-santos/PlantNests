using PlantNests.Controllers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace PlantNests.Models
{
    public class Registration:ApplicationUser
    {
        [Required]
        [MaxLength(200)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password not match.")]
        public string ConfirmPassword {get; set; }
        [Required]
        public string ContactNumber { get; set; }

        [NotMapped]
        public string RoleName { get; set; }
    }
}
