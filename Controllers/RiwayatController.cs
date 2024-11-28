using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCarWeb.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RentCarWeb.Controllers
{
    public class RiwayatController : Controller
    {
        private readonly CarHomeContext _context;

        public RiwayatController(CarHomeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var username = HttpContext.Session.GetString("Username");
            ViewData["Username"] = username;

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Login", "Account");
            }

            var rentalHistory = await _context.Rental
                .Where(rental => rental.UserId == userId)
                .Join(_context.Cars,
                    rental => rental.CarId,
                    car => car.Car_Id,
                    (rental, car) => new RentalHistoryViewModel
                    {
                        RentalId = rental.RentalId,
                        PickupDate = rental.PickupDate,
                        ReturnDate = rental.ReturnDate,
                        CarName = car.name,
                        Year = car.year,
                        PricePerDay = car.price_per_day,
                        TotalDays = EF.Functions.DateDiffDay(rental.PickupDate, rental.ReturnDate) + 1,
                        TotalPrice = rental.TotalPrice + car.price_per_day,
                        Status = rental.status
                    })
                .ToListAsync();

            return View(rentalHistory);
        }
    }
}