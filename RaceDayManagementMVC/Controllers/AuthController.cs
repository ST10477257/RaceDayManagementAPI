using Microsoft.AspNetCore.Mvc;
using RaceDayManagementMVC.Models;
using System.Net.Http.Json;

namespace RaceDayManagementMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("RaceDayApi");

            var response = await client.PostAsJsonAsync(
                "api/Auth/login", model);
            if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
            {
                var apiCookie = cookies
                    .FirstOrDefault()?
                    .Split(';')[0];

                if (!string.IsNullOrEmpty(apiCookie))
                {
                    HttpContext.Session.SetString("ApiSessionCookie", apiCookie);
                }
            }

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            var user = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (user == null)
            {
                ModelState.AddModelError("", "Unable to process login.");
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetString("FirstName", user.FirstName);

            return user.Role == "Organiser"
                ? RedirectToAction("Index", "Organiser")
                : RedirectToAction("Index", "Participant");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("RaceDayApi");

            var registrationData = new
            {
                firstName = model.FirstName,
                lastName = model.LastName,
                email = model.Email,
                password = model.Password,
                phoneNumber = model.PhoneNumber,
                role = "Participant"
            };

            var response = await client.PostAsJsonAsync(
                "api/Auth/register", registrationData);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Registration failed. Check your details.");
                return View(model);
            }

            TempData["Success"] = "Registration successful. Please log in.";

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        private class LoginResponse
        {
            public int UserId { get; set; }
            public string Role { get; set; } = "";
            public string FirstName { get; set; } = "";
        }
    }
}
