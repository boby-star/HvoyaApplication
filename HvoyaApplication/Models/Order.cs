namespace HvoyaApplication.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal OrderTotal { get; set; }

        public string UserId { get; set; }
        public AppUser? User { get; set; }

        public string Status { get; set; } = "Очікує замовлення";

        public string FullName { get; set; } // ім'я замовника
        public string Address { get; set; } // адреса доставки
        public string PhoneNumber { get; set; } // номер телефону

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
