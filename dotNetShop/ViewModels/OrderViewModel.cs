using dotNetShop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace dotNetShop.ViewModels
{
    public class OrderViewModel
    {
        public CartViewModel Cart { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public Address Address { get; set; }

        [Display(Name = "Payment Method")]
        [Required]
        public PaymentMethod? PaymentMethod { get; set; }

        public SelectList PaymentMethods { get; set; }
    }

    public enum PaymentMethod
    {
        [Display(Name = "Credit Card")]
        CreditCard,
        [Display(Name = "Debit Card")]
        DebitCard,
        [Display(Name = "PayPal")]
        PayPal,
        [Display(Name = "Blik")]
        Blik
    }
}
