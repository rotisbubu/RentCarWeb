using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCarWeb.Models;
using System.Diagnostics;

namespace RentCarWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(DateTime? pickupDate, DateTime? returnDate, string yearFilter)
        {
            var username = HttpContext.Session.GetString("Username");
            ViewData["Username"] = username;

            if (pickupDate.HasValue && returnDate.HasValue)
            {
                HttpContext.Session.SetString("pickupDate", pickupDate.Value.ToString("yyyy-MM-dd"));
                HttpContext.Session.SetString("returnDate", returnDate.Value.ToString("yyyy-MM-dd"));
            }

            try
            {
                int[] carYears = { 2019,2018,2017,2016,2015};

                ViewBag.Years = carYears;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching cars: {ex.Message}");
                return Content("Error fetching cars.");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
