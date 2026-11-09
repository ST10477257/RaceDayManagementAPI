using Microsoft.AspNetCore.Mvc;
using RaceDayManagementMVC.Models;
using RaceDayManagementMVC.Services;

namespace RaceDayManagementMVC.Controllers
{
    public class RoutesController : Controller
    {
        private readonly RaceDayApiService _apiService;

        public RoutesController(RaceDayApiService apiService)
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
                var routes = await _apiService
                    .GetAllAsync<RouteViewModel>("api/Routes");

                return View(routes);
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to connect to the RaceDay API.";
                return View(new List<RouteViewModel>());
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
        public async Task<IActionResult> Create(RouteViewModel route)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
                return View(route);

            var response = await _apiService.PostAsync(
                "api/Routes", route);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            var error = await response.Content.ReadAsStringAsync();

            ModelState.AddModelError("",
                $"API Error ({(int)response.StatusCode}): {error}");

            return View(route);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            var route = await _apiService.GetByIdAsync<RouteViewModel>(
                "api/Routes", id);

            if (route == null)
                return NotFound();

            return View(route);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RouteViewModel route)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
                return View(route);

            var response = await _apiService.PutAsync(
                "api/Routes", id, route);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            var error = await response.Content.ReadAsStringAsync();

            ModelState.AddModelError("",
                $"API Error ({(int)response.StatusCode}): {error}");

            return View(route);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            var response = await _apiService.DeleteAsync(
                "api/Routes", id);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] =
                    $"Unable to delete route. API returned {(int)response.StatusCode}.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}