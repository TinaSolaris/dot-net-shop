using dotNetShop.Data;
using dotNetShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace dotNetShop.Controllers
{
    [Authorize(Policy = Settings.PolicyName.NonAdmin)]
    public class CartController : Controller
    {
        private readonly ShopDbContext _context;
        private readonly ICartService _cartService;

        public CartController(
            ShopDbContext context,
            ICartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        public IActionResult Index()
        {
            var cartViewModel = _cartService.GetCartViewModel();

            return View(cartViewModel);
        }

        [HttpGet]
        public IActionResult AddToCart(int articleId)
        {
            var article = _context.Articles.Find(articleId);
            if (article == null)
                return RedirectToAction(nameof(ShopController.Index), "Shop");

            Dictionary<int, int> cartItems = _cartService.GetCartItems();

            if (cartItems.ContainsKey(articleId))
                cartItems[articleId] = cartItems[articleId] + 1;    
            else         
                cartItems.Add(articleId, 1);

            _cartService.SaveCartItems(cartItems);

            return RedirectToAction(nameof(ShopController.List), "Shop", new { categoryId = article.CategoryId });
        }

        [HttpGet]
        public IActionResult UpdateCart(int articleId, int quantity)
        {
            if (quantity <= 0)
                return RemoveFromCart(articleId);

            Dictionary<int, int> cartItems = _cartService.GetCartItems();

            if (cartItems.ContainsKey(articleId))
                cartItems[articleId] = quantity;
            else
                cartItems.Add(articleId, quantity);

            _cartService.SaveCartItems(cartItems);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult RemoveFromCart(int articleId)
        {
            Dictionary<int, int> cartItems = _cartService.GetCartItems();

            cartItems.Remove(articleId);

            _cartService.SaveCartItems(cartItems);

            return RedirectToAction(nameof(Index));
        }
    }
}