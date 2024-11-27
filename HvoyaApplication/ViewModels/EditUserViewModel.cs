using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HvoyaApplication.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Повне ім'я є обов'язковим")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email є обов'язковим")]
        [EmailAddress(ErrorMessage = "Некоректний формат Email")]
        public string Email { get; set; }

        public IList<string> Roles { get; set; } = new List<string>();
    }
}
