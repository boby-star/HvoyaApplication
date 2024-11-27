using HvoyaApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HvoyaApplication.Models.Interfaces;
using HvoyaApplication.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HvoyaApplication.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(IOrderRepository orderRepository, IShoppingCartRepository shoppingCartRepository, UserManager<AppUser> userManager)
        {
            _orderRepository = orderRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = await _shoppingCartRepository.GetActiveCartAsync(user.Id);
            if (cart == null || !cart.ShoppingCartItems.Any())
            {
                TempData["ErrorMessage"] = "Ваш кошик порожній!";
                return RedirectToAction("Index", "Home");
            }

            var model = new CheckoutViewModel
            {
                FullName = user.FullName ?? string.Empty,
                Address = string.Empty,
                PhoneNumber = string.Empty
            };

            ViewBag.ShoppingCart = cart;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var cart = await _shoppingCartRepository.GetActiveCartAsync(user.Id);
            if (cart == null || !cart.ShoppingCartItems.Any())
            {
                ModelState.AddModelError("", "Ваш кошик порожній!");
                return View(model);
            }

            try
            {
                var order = new Order
                {
                    UserId = user.Id,
                    FullName = model.FullName,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    OrderDate = DateTime.Now,
                    OrderTotal = cart.ShoppingCartItems.Sum(i => i.Dessert.Price * i.Quantity),
                    OrderItems = cart.ShoppingCartItems.Select(i => new OrderItem
                    {
                        DessertId = i.DessertId,
                        Quantity = i.Quantity,
                        Price = i.Dessert.Price
                    }).ToList(),
                    Status = "Очікує замовлення"
                };

                await _orderRepository.CreateOrderAsync(order);
                await _shoppingCartRepository.ClearCartAsync(user.Id);

                return RedirectToAction("CheckoutComplete");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Сталася помилка при оформленні замовлення. Спробуйте ще раз.");
                return View(model);
            }
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = "Дякуємо за ваше замовлення! Ваше замовлення обробляється.";
            return View();
        }
    }
}
