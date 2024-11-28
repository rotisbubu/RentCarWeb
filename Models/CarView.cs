using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentCarWeb.Models
{
    public class CarView
    {
        public string Car_Id { get; set; }
        public string name { get; set; }
        public string model { get; set; }
        public int year { get; set; }
        public string license_plate { get; set; }
        public int number_of_car_seats { get; set; }
        public string transmission { get; set; }
        public decimal price_per_day { get; set; }
        public bool status { get; set; }
        public string ImageLink { get; set; }
    }
}
