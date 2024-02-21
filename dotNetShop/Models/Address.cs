using System.ComponentModel.DataAnnotations;

namespace dotNetShop.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Too short value")]
        [MaxLength(200, ErrorMessage = "Too long value, should not exceed {0} characters")]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        [MaxLength(200, ErrorMessage = "Too long value, should not exceed {0} characters")]
        public string AddressLine2 { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Too short value")]
        [MaxLength(200, ErrorMessage = "Too long value, should not exceed {0} characters")]
        public string City { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Too short value")]
        [MaxLength(200, ErrorMessage = "Too long value, should not exceed {0} characters")]
        public string Country { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Too short value")]
        [Display(Name = "Postal Code")]
        [MaxLength(10, ErrorMessage = "Too long value, should not exceed {0} characters")]
        public string PostalCode { get; set; }
    }
}
