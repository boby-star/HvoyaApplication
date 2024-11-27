using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HvoyaApplication.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        public ICollection<ShoppingCart> ShoppingCarts {  get; set; }
    }
}
