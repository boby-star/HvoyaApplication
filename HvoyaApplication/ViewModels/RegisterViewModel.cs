using System.ComponentModel.DataAnnotations;

namespace HvoyaApplication.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ім'я є обов'язковим")]
        [Display(Name = "Повне ім'я")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Електронна пошта є обов'язковою")]
        [EmailAddress(ErrorMessage = "Введіть коректну електронну пошту")]
        [Display(Name = "Електронна пошта")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль є обов'язковим")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Підтвердження пароля є обов'язковим")]
        [DataType(DataType.Password)]
        [Display(Name = "Підтвердити пароль")]
        [Compare("Password", ErrorMessage = "Паролі не збігаються")]
        public string ConfirmPassword { get; set; }
    }
}
