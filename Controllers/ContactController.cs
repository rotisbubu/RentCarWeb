using Microsoft.AspNetCore.Mvc;

namespace RentCarWeb.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");
            ViewData["Username"] = username;
            return View();
        }
    }
}
