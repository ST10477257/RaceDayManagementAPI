using Microsoft.AspNetCore.Mvc;
using RaceDayManagementMVC.Models;
using RaceDayManagementMVC.Services;

namespace RaceDayManagementMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly RaceDayApiService _apiService;

        public CategoriesController(RaceDayApiService apiService)
        {
            _apiService = apiService;
        }

        private bool IsOrganiser()
        {
            return HttpContext.Session.GetInt32("UserId") != null
                && HttpContext.Session.GetString("Role") == "Organiser";
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _apiService
                    .GetAllAsync<CategoryViewModel>("api/Categories");

                return View(categories);
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<CategoryViewModel>());
            }
        }

        public IActionResult Create()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel category)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
                return View(category);

            try
            {
                var response = await _apiService.PostAsync(
                    "api/Categories", category);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                var error = await response.Content.ReadAsStringAsync();

                ModelState.AddModelError("",
                    $"API Error ({(int)response.StatusCode}): {error}");

                return View(category);
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(category);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            try
            {
                var category = await _apiService.GetByIdAsync<CategoryViewModel>(
                    "api/Categories", id);

                if (category == null)
                    return NotFound();

                return View(category);
            }
            catch (HttpRequestException)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, CategoryViewModel category)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
                return View(category);

            try
            {
                var response = await _apiService.PutAsync(
                    "api/Categories", id, category);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                var error = await response.Content.ReadAsStringAsync();

                ModelState.AddModelError("",
                    $"API Error ({(int)response.StatusCode}): {error}");

                return View(category);
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(category);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            var response = await _apiService.DeleteAsync(
                "api/Categories", id);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] =
                    $"Unable to delete category. API returned {(int)response.StatusCode}.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}