using HvoyaApplication.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HvoyaApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using HvoyaApplication.Models.Repositories;
using Microsoft.EntityFrameworkCore;
using HvoyaApplication.Data;

namespace HvoyaApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller

    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IDessertRepository _dessertRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;

        public AdminController(UserManager<AppUser> userManager, ApplicationDbContext context, ICategoryRepository categoryRepository, IDessertRepository dessertRepository, IOrderRepository orderRepository, IUserRepository userRepository)
        {
            _dessertRepository = dessertRepository;
            _orderRepository = orderRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ManageUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return View(users);
        }

        public async Task<IActionResult> ManageOrders()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            return View(orders);
        }

        public async Task<IActionResult> ManageDesserts()
        {
            var desserts = await _context.Desserts.Include(d => d.Category).ToListAsync();
            return View(desserts);
        }

        [HttpGet]
        public IActionResult AddDessert()
        {
            ViewBag.CategoryList = new SelectList(new List<SelectListItem>
            {
            new SelectListItem { Value = "1", Text = "Торти" },
            new SelectListItem { Value = "2", Text = "Кекси" }
            }, "Value", "Text");

            ViewData["Title"] = "Додати десерт";
            return View(new Dessert());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDessert([Bind("Name,Description,Price,ImageUrl,CategoryId,IsAvailable")] Dessert dessert)
        {
            ModelState.Remove("Category");
            ModelState.Remove("ShoppingCartItems");
            ModelState.Remove("OrderItems");

            if (!ModelState.IsValid)
            {
                ViewBag.CategoryList = new SelectList(await _categoryRepository.GetAllCategoriesAsync(), "CategoryId", "CategoryName");
                return View(dessert);
            }

            try
            {
                await _dessertRepository.AddDessertAsync(dessert);
                TempData["SuccessMessage"] = "Десерт успішно додано.";
                return RedirectToAction("ManageDesserts");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Сталася помилка: {ex.Message}");
                return View(dessert);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditDessert(int id)
        {
            var dessert = await _context.Desserts.FindAsync(id);
            if (dessert == null)
                return NotFound();

            var categories = await _context.Categories.ToListAsync();
            ViewBag.CategoryList = new SelectList(categories, "CategoryId", "CategoryName", dessert.CategoryId);

            ViewData["Title"] = "Редагувати десерт";
            return View(dessert);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDessert([Bind("DessertId,Name,Description,Price,ImageUrl,CategoryId,IsAvailable")] Dessert dessert)
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.CategoryList = new SelectList(categories, "CategoryId", "CategoryName", dessert.CategoryId);

            ModelState.Remove("Category");
            ModelState.Remove("ShoppingCartItems");
            ModelState.Remove("OrderItems");

            if (!ModelState.IsValid)
            {
                return View(dessert);
            }

            try
            {
                _context.Desserts.Update(dessert);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Десерт успішно оновлено.";
                return RedirectToAction("ManageDesserts");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Сталася помилка: " + ex.Message);
                return View(dessert);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDessert(int id)
        {
            var dessert = await _context.Desserts.FindAsync(id);
            if (dessert == null)
            {
                TempData["ErrorMessage"] = "Десерт не знайдено.";
                return RedirectToAction("ManageDesserts");
            }

            _context.Desserts.Remove(dessert);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Десерт успішно видалено.";
            return RedirectToAction("ManageDesserts");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string newStatus)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order != null)
            {
                order.Status = newStatus;
                await _orderRepository.UpdateOrderAsync(order);
                TempData["SuccessMessage"] = "Статус замовлення оновлено.";
            }
            else
            {
                TempData["ErrorMessage"] = "Замовлення не знайдено.";
            }

            return RedirectToAction("ManageOrders");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            await _orderRepository.DeleteOrderAsync(orderId);
            TempData["SuccessMessage"] = "Замовлення успішно видалено.";
            return RedirectToAction("ManageOrders");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Користувача не знайдено.";
                return RedirectToAction("ManageUsers");
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Користувача успішно видалено.";
            }
            else
            {
                TempData["ErrorMessage"] = "Сталася помилка при видаленні користувача.";
            }

            return RedirectToAction("ManageUsers");
        }
    }
}
