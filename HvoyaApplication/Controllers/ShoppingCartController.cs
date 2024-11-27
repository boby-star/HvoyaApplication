using HvoyaApplication.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using HvoyaApplication.Models;
using HvoyaApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HvoyaApplication.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IDessertRepository _dessertRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartController(IDessertRepository dessertRepository, IShoppingCartRepository shoppingCartRepository)
        {
            _dessertRepository = dessertRepository;
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "Користувач не авторизований!";
                return RedirectToAction("Index", "Home");
            }

            var cart = await _shoppingCartRepository.GetActiveCartAsync(userId);

            var viewModel = new ShoppingCartViewModel
            {
                ShoppingCartItems = cart.ShoppingCartItems.ToList(),
                ShoppingCartTotal = cart.ShoppingCartItems.Sum(i => i.Dessert.Price * i.Quantity)
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int dessertId, int quantity = 1)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "Користувач не авторизований!";
                return RedirectToAction("Index", "Home");
            }

            var selectedDessert = await _dessertRepository.GetDessertByIdAsync(dessertId);

            if (selectedDessert == null)
            {
                TempData["Error"] = "Десерт не знайдено!";
                return RedirectToAction("Index", "Home");
            }

            await _shoppingCartRepository.AddToCartAsync(userId, selectedDessert, quantity);

            TempData["Success"] = $"{selectedDessert.Name} додано до кошика!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int dessertId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "Користувач не авторизований!";
                return RedirectToAction("Index", "Home");
            }

            await _shoppingCartRepository.RemoveFromCartAsync(userId, dessertId);

            TempData["Success"] = "Десерт видалено з кошика!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "Користувач не авторизований!";
                return RedirectToAction("Index", "Home");
            }

            await _shoppingCartRepository.ClearCartAsync(userId);

            TempData["Success"] = "Кошик очищено!";
            return RedirectToAction("Index");
        }
    }
}
