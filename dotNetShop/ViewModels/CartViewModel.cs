using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace dotNetShop.ViewModels
{
    public class CartViewModel
    {
        public IEnumerable<CartItemViewModel> CartItems { get; set; }

        [Display(Name = "Total Cart Price")]
        [DataType(DataType.Currency)]
        public double TotalCartPrice { get; set; }
    }
}
