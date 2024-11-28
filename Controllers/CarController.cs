using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RentCarWeb.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RentCarWeb.Controllers
{
    public class CarController : Controller
    {
        private readonly CarHomeContext _context;

        public CarController(CarHomeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(DateTime? pickupDate, DateTime? returnDate, string yearFilter, string sortOrder)
        {
            var username = HttpContext.Session.GetString("Username");
            ViewData["Username"] = username;
            ViewData["UserEmail"] = HttpContext.Session.GetString("UserEmail");

            if (pickupDate.HasValue && returnDate.HasValue)
            {
                if (pickupDate > returnDate)
                {
                    return RedirectToAction("Index", "Home");
                }
                HttpContext.Session.SetString("pickupDate", pickupDate.Value.ToString("yyyy-MM-dd"));
                HttpContext.Session.SetString("returnDate", returnDate.Value.ToString("yyyy-MM-dd"));
            }



            try
            {
                var carYears = await _context.Cars
                                             .Where(car => car.status == true)
                                             .Select(car => car.year)
                                             .Distinct()
                                             .OrderByDescending(year => year)
                                             .ToListAsync();
                ViewBag.Years = carYears;

                var carsQuery = _context.Cars.Where(car => car.status == true);

                if (!string.IsNullOrEmpty(yearFilter) && yearFilter != "all")
                {
                    if (int.TryParse(yearFilter, out int selectedYear))
                    {
                        carsQuery = carsQuery.Where(car => car.year == selectedYear);
                    }
                }

                carsQuery = sortOrder switch
                {
                    "Desc" => carsQuery.OrderByDescending(car => car.price_per_day),
                    _ => carsQuery.OrderBy(car => car.price_per_day)
                };

                var carDetailsWithImages = await carsQuery
                    .Join(
                        _context.Images,
                        car => car.Car_Id,
                        image => image.Car_id,
                        (car, image) => new CarView
                        {
                            Car_Id = car.Car_Id,
                            name = car.name,
                            model = car.model,
                            year = car.year,
                            license_plate = car.license_plate,
                            number_of_car_seats = car.number_of_car_seats,
                            transmission = car.transmission,
                            price_per_day = car.price_per_day,
                            status = car.status,
                            ImageLink = image.image_link
                        }
                    )
                    .ToListAsync();

                return View(carDetailsWithImages);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching cars: {ex.Message}");
                return Content("Error fetching cars. Please check the database connection and try again.");
            }
        }

        public async Task<IActionResult> Rental(string id, string imageLink)
        {
            var username = HttpContext.Session.GetString("Username");
            ViewData["Username"] = username;
            ViewData["UserEmail"] = HttpContext.Session.GetString("UserEmail");
            
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Login", "Account");
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            var pickupDates = HttpContext.Session.GetString("pickupDates");
            var returnDates = HttpContext.Session.GetString("returnDates");
            var pickupDate = HttpContext.Session.GetString("pickupDate");
            var returnDate = HttpContext.Session.GetString("returnDate");

            if (pickupDates == null || returnDates == null)
            { 
                if(pickupDate == null || returnDate == null)
                {   
                    return RedirectToAction("Index");
                }
            }
            else
            {
                pickupDate = HttpContext.Session.GetString("pickupDates");
                returnDate = HttpContext.Session.GetString("returnDates");
            }

            ViewBag.PickupDate = DateTime.Parse(pickupDate);
            ViewBag.ReturnDate = DateTime.Parse(returnDate);

            ViewData["ImageLink"] = imageLink;

            return View(car);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmRental(string carId, DateTime pickupDate, DateTime returnDate)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var car = await _context.Cars.FindAsync(carId);
            if (car == null)
            {
                return NotFound();
            }

            var totalDays = (returnDate - pickupDate).Days;
            var totalPrice = car.price_per_day * totalDays;
            var newRental = new RiwayatSewa
            {
                RentalId = _context.GetNextRentalId(),
                UserId = HttpContext.Session.GetString("UserId"),
                CarId = carId,
                PickupDate = pickupDate,
                ReturnDate = returnDate,
                TotalPrice = totalPrice,
                status = false
            };

            _context.Rental.Add(newRental);
            _context.SaveChanges();
            return RedirectToAction("Index", "Riwayat");
        }

    }
}
