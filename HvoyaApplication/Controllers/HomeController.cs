using HvoyaApplication.Models;
using HvoyaApplication.Models.Interfaces;
using HvoyaApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HvoyaApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDessertRepository _dessertRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IReviewRepository _reviewRepository;

        public HomeController(ILogger<HomeController> logger, IDessertRepository dessertRepository, ICategoryRepository categoryRepository, IReviewRepository reviewRepository)
        {
            _logger = logger;
            _dessertRepository = dessertRepository;
            _categoryRepository = categoryRepository;
            _reviewRepository = reviewRepository;
        }

        public async Task<IActionResult> Index(string category, string search)
        {
            var desserts = await _dessertRepository.GetAllDessertsAsync();

            if (!string.IsNullOrEmpty(category) && category != "Всі категорії")
            {
                desserts = desserts.Where(d => d.Category.CategoryName == category);
            }

            if (!string.IsNullOrEmpty(search))
            {
                desserts = desserts.Where(d => d.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                                d.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            var dessertViewModels = desserts.Select(d => new DessertDetailsViewModel
            {
                DessertId = d.DessertId,
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                ImageUrl = d.ImageUrl
            });

            var viewModel = new HomeViewModel
            {
                Desserts = dessertViewModels,
                CurrentCategory = category ?? "Всі категорії"
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddReview(int dessertId, string comment, int rating)
        {
            if (rating < 1 || rating > 5)
            {
                ModelState.AddModelError("Rating", "Рейтинг повинен бути між 1 та 5");
                return RedirectToAction("Details", new { id = dessertId });
            }

            var review = new Review
            {
                DessertId = dessertId,
                Comment = comment,
                Rating = rating,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            await _reviewRepository.AddReviewAsync(review);
            return RedirectToAction("Details", new { id = dessertId });
        }

        public async Task<IActionResult> Details(int id)
        {
            var dessert = await _dessertRepository.GetDessertByIdAsync(id);
            if (dessert == null)
            {
                return View("Error");
            }

            var reviews = await _reviewRepository.GetReviewsByDessertIdAsync(id);

            var model = new DessertDetailsViewModel
            {
                DessertId = dessert.DessertId,
                Name = dessert.Name,
                Description = dessert.Description,
                Price = dessert.Price,
                ImageUrl = dessert.ImageUrl,
                Reviews = reviews.ToList()
            };

            return View(model);
        }

        public async Task<IActionResult> Search(string searchString)
        {
            var desserts = string.IsNullOrEmpty(searchString)
                ? await _dessertRepository.GetAllDessertsAsync()
                : await _dessertRepository.SearchDessertsAsync(searchString);

            var dessertViewModels = desserts.Select(d => new DessertDetailsViewModel
            {
                DessertId = d.DessertId,
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                ImageUrl = d.ImageUrl
            });

            return View("Index", new HomeViewModel
            {
                Desserts = dessertViewModels,
                CurrentCategory = "SearchResults"
        });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
