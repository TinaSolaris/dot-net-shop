using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace dotNetShop.Models
{
    public class OrderUser
    {
        [Required]
        [MaxLength(450)]
        public string Id { get; set; }

        [Required]
        public IdentityUser User { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
