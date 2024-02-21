using System.ComponentModel.DataAnnotations;
using dotNetShop.Models;

namespace dotNetShop.ViewModels
{
    public class CartItemViewModel
    {
        public Article Article { get; set; }

        public int Quantity { get; set; }

        [Display(Name = "Total Price")]
        [DataType(DataType.Currency)]
        public double TotalPrice { get; set; }
    }
}