using System.ComponentModel.DataAnnotations;

namespace HvoyaApplication.Models
{
    public class ShoppingCartItem
    {
        public int ShoppingCartItemId { get; set; }

        [Range(1, 100, ErrorMessage = "Кількість повинна бути від 1 до 100.")]
        public int Quantity { get; set; }

        public int DessertId { get; set; }
        public Dessert Dessert { get; set; }

        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}

