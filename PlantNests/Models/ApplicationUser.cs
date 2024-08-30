using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PlantNests.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string ContactNumber { get; set; }
    }
}
