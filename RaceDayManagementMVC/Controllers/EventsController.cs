using Microsoft.AspNetCore.Mvc;
using RaceDayManagementMVC.Models;
using RaceDayManagementMVC.Services;

namespace RaceDayManagementMVC.Controllers
{
    public class EventsController : Controller
    {
        private readonly RaceDayApiService _apiService;

        public EventsController(RaceDayApiService apiService)
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
                var events = await _apiService
                    .GetAllAsync<EventViewModel>("api/Events");

                return View(events);
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to connect to the RaceDay API.";
                return View(new List<EventViewModel>());
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
        public async Task<IActionResult> Create(EventViewModel eventModel)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
                return View(eventModel);

            var response = await _apiService.PostAsync(
                "api/Events", eventModel);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            var error = await response.Content.ReadAsStringAsync();

            ModelState.AddModelError("",
                $"API Error ({(int)response.StatusCode}): {error}");

            return View(eventModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            var eventModel = await _apiService.GetByIdAsync<EventViewModel>(
                "api/Events", id);

            if (eventModel == null)
                return NotFound();

            return View(eventModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, EventViewModel eventModel)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
                return View(eventModel);

            var response = await _apiService.PutAsync(
                "api/Events", id, eventModel);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            var error = await response.Content.ReadAsStringAsync();

            ModelState.AddModelError("",
                $"API Error ({(int)response.StatusCode}): {error}");

            return View(eventModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            var response = await _apiService.DeleteAsync(
                "api/Events", id);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] =
                    $"Unable to delete event. API returned {(int)response.StatusCode}.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}