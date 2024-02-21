using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace dotNetShop.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Too short name")]
        [MaxLength(200, ErrorMessage = "Too long name, should not exceed {0} characters")]
        [Display(Name = "Category")]
        public string Name { get; set; }
    }
}
