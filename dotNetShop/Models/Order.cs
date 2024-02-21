using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace dotNetShop.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(450)]
        public string UserId { get; set; }

        [Required]
        public int AddressId { get; set; }

        [Required]
        [Display(Name = "Total Order Price")]
        [DataType(DataType.Currency)]
        public double TotalOrderPrice { get; set; }

        [Required]
        public OrderUser User { get; set; }

        [Required]
        public Address Address { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        [Required]
        public ICollection<OrderItem> OrderItems { get; set;}
    }
}
