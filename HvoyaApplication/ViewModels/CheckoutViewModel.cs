using HvoyaApplication.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace HvoyaApplication.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Ім'я є обов'язковим")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Адреса доставки є обов'язковою")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Номер телефону є обов'язковим")]
        public string PhoneNumber { get; set; }
    }


}
