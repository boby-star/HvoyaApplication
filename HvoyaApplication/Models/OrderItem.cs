using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HvoyaApplication.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        [Range(1, 100, ErrorMessage = "Кількість має бути в межах від 1 до 100.")]
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public int OrderId {  get; set; }
        public Order Order { get; set; }

        public int DessertId { get; set; }
        public Dessert Dessert { get; set; }
    }
}
