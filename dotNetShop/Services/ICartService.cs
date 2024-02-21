using dotNetShop.ViewModels;
using System.Collections.Generic;

namespace dotNetShop.Services
{
    public interface ICartService
    {
        Dictionary<int, int> GetCartItems();

        void SaveCartItems(Dictionary<int, int> cartItems);

        bool IsCartEmpty();

        CartViewModel GetCartViewModel();

        void ClearCart();
    }
}
