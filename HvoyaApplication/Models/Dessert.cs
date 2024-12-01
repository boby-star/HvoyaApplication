using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HvoyaApplication.Utilities;

namespace HvoyaApplication.Models
{
    public class Dessert
    {
        public int DessertId { get; set; }
        [Required(ErrorMessage = "Назва є обов'язковою")]
        [Display(Name = "Назва")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Опис є обов'язковим")]
        [Display(Name = "Опис")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Ціна є обов'язковою")]
        [ValidatePositivePrice(ErrorMessage = "Ціна повинна бути більше 0.")]
        [Display(Name = "Ціна")]
        public int Price { get; set; }


        [Display(Name = "Зображення")]
        public string ImageUrl { get; set; }

        [Display(Name = "Доступність")]
        public bool IsAvailable { get; set; } = true;

        [Required(ErrorMessage = "Категорія є обов'язковою")]
        [Display(Name = "Категорія")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
        public ICollection<OrderItem> OrderItems {  get; set; }
    }
}
