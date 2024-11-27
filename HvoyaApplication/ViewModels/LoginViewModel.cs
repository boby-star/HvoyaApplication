using System.ComponentModel.DataAnnotations;

namespace HvoyaApplication.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Електронна пошта є обов'язковою")]
        [EmailAddress(ErrorMessage = "Введіть коректну електронну пошту")]
        [Display(Name = "Електронна пошта")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль є обов'язковим")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запам'ятати мене")]
        public bool RememberMe { get; set; }
    }
}
