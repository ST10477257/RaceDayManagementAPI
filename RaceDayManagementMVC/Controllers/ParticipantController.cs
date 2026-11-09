using Microsoft.AspNetCore.Mvc;

namespace RaceDayManagementMVC.Controllers
{
    public class ParticipantController : Controller
    {
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null)
                return RedirectToAction("Login", "Auth");

            if (role != "Participant")
                return RedirectToAction("Login", "Auth");

            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");

            return View();
        }
    }
}