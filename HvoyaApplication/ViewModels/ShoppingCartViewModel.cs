using HvoyaApplication.Models;

namespace HvoyaApplication.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
        public decimal ShoppingCartTotal { get; set; }

    }
}
