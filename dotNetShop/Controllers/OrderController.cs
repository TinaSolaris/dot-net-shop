using dotNetShop.Data;
using dotNetShop.Models;
using dotNetShop.Services;
using dotNetShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dotNetShop.Controllers
{
    [Authorize(Policy = Settings.PolicyName.AuthNonAdmin)]
    public class OrderController : Controller
    {
        private readonly ShopDbContext _context;
        private readonly ICartService _cartService;

        public OrderController(
            ShopDbContext context, 
            ICartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        private void PopulateOrderViewModel(OrderViewModel model)
        {
            model.Cart = _cartService.GetCartViewModel();

            var selectListItems = new List<SelectListItem>();
            selectListItems.Add(
                new SelectListItem
                {
                    Text = "Please select a payment method",
                    Value = String.Empty
                });

            Type enumType = typeof(PaymentMethod);

            foreach (PaymentMethod method in Enum.GetValues(typeof(PaymentMethod)))
            {
                selectListItems.Add(
                    new SelectListItem
                    {
                        Text = GetEnumDisplayName(method),
                        Value = method.ToString()
                    });
            }

            model.PaymentMethods = new SelectList(
                selectListItems,
                "Value",
                "Text",
                model.PaymentMethod != null ? model.PaymentMethod.ToString() : "");
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (_cartService.IsCartEmpty())
                return RedirectToAction("Index", "Cart");

            var orderViewModel = new OrderViewModel();
            PopulateOrderViewModel(orderViewModel);
       
            return View(orderViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirmation(OrderViewModel orderViewModel)
        {
            PopulateOrderViewModel(orderViewModel);

            if (!ModelState.IsValid)
                return View(nameof(Index), orderViewModel);

            var address = new Address
            {
                AddressLine1 = orderViewModel.Address.AddressLine1,
                AddressLine2 = orderViewModel.Address.AddressLine2,
                City = orderViewModel.Address.City,
                Country = orderViewModel.Address.Country,
                PostalCode = orderViewModel.Address.PostalCode
            };

            OrderUser user = GetUser(orderViewModel);

            var order = new Order
            {
                User = user,
                Address = address,
                TotalOrderPrice = orderViewModel.Cart.TotalCartPrice,
                PaymentMethod = GetEnumDisplayName(orderViewModel.PaymentMethod),
                OrderItems = new List<OrderItem>()
            };

            foreach (var cartItem in orderViewModel.Cart.CartItems)
            {
                var orderItem = new OrderItem
                {
                    Quantity = cartItem.Quantity,
                    TotalPrice = cartItem.TotalPrice,
                    ArticleId = cartItem.Article.Id,
                };

                order.OrderItems.Add(orderItem);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            _cartService.ClearCart();

            return View(order);
        }

        private OrderUser GetUser(OrderViewModel orderViewModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var identityUser = _context.Users.First(x => x.Id == userId);
            var user = _context.OrderUsers.FirstOrDefault(x => x.Id == userId);

            if (user == null)
            {
                user = new OrderUser
                {
                    Id = identityUser.Id,
                    User = identityUser
                };
            }

            user.FirstName = orderViewModel.FirstName;
            user.LastName = orderViewModel.LastName;

            return user;
        }

        private string GetEnumDisplayName(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DisplayAttribute)Attribute.GetCustomAttribute(field, typeof(DisplayAttribute));

            return attribute == null ? value.ToString() : attribute.Name;
        }
    }
}
