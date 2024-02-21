using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace dotNetShop.ViewModels
{
    public class ShopIndexViewModel
    {
        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set;}

        public SelectList Categories { get; set; }
    }
}
