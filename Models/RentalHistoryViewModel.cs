using System.ComponentModel.DataAnnotations;

namespace RentCarWeb.Models
{
    public class RentalHistoryViewModel
    {
        public string RentalId { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string CarName { get; set; }
        public int Year { get; set; }
        public decimal PricePerDay { get; set; }
        public int TotalDays { get; set; }
        public decimal TotalPrice { get; set; }
        public bool Status { get; set; }
    }
}
