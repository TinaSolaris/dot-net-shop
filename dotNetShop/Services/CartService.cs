using dotNetShop.Data;
using dotNetShop.ViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dotNetShop.Services
{
    public class CartService : ICartService
    {
        private const string CART_COOKIE_NAME = "CartItems";
        private const int CART_COOKIE_LIFETIME_DAYS = 7;

        private readonly IArticleService _articleService;
        private readonly ShopDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private HttpContext HttpContext
        {
            get { return _httpContextAccessor.HttpContext; }
        }

        public CartService(IHttpContextAccessor httpContextAccessor, IArticleService articleService, ShopDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _articleService = articleService;
            _context = context;
        }

        public Dictionary<int, int> GetCartItems()
        {
            try
            {
                return HttpContext.Request.Cookies.ContainsKey(CART_COOKIE_NAME)
                    ? JsonConvert.DeserializeObject<Dictionary<int, int>>(HttpContext.Request.Cookies[CART_COOKIE_NAME])
                    : new Dictionary<int, int>();
            }
            catch (Exception)
            {
                // The error could be logged on demand.
                return new Dictionary<int, int>();
            }
        }

        public void SaveCartItems(Dictionary<int, int> cartItems)
        {
            HttpContext.Response.Cookies.Append(
                CART_COOKIE_NAME,
                JsonConvert.SerializeObject(cartItems),
                new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(CART_COOKIE_LIFETIME_DAYS)
                });
        }

        public bool IsCartEmpty()
        {
            return !GetCartItems().Any();
        }

        public CartViewModel GetCartViewModel()
        {
            Dictionary<int, int> cartItems = GetCartItems();

            var cartItemViewModels = new List<CartItemViewModel>();
            bool isDeleted = false;

            foreach (var cartItem in cartItems)
            {
                var article = _context.Articles.Find(cartItem.Key);

                if (article != null)
                {
                    _articleService.ExtendViewModel(article);

                    cartItemViewModels.Add(new CartItemViewModel
                    {
                        Article = article,
                        Quantity = cartItem.Value,
                        TotalPrice = article.Price * cartItem.Value
                    });
                }
                else
                {
                    cartItems.Remove(cartItem.Key);
                    isDeleted = true;
                }
            }

            if (isDeleted)
                SaveCartItems(cartItems);

            var cartViewModel = new CartViewModel
            {
                CartItems = cartItemViewModels,
                TotalCartPrice = cartItemViewModels.Sum(item => item.Quantity * item.Article.Price)
            };

            return cartViewModel;
        }

        public void ClearCart()
        {
            //HttpContext.Response.Cookies.Append(
            //    CART_COOKIE_NAME,
            //    JsonConvert.SerializeObject(new Dictionary<int, int>()),
            //    new CookieOptions
            //    {
            //        Expires = DateTime.Now.AddDays(CART_COOKIE_LIFETIME_DAYS)
            //    });

            HttpContext.Response.Cookies.Delete(CART_COOKIE_NAME);
        }
    }
}
