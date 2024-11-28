using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCarWeb.Models
{
    [Table("TrRental")]
    public class RiwayatSewa
    {
        [Key]
        [Column("Rental_id")]
        public string RentalId { get; set; }
        [Column("customer_id")]
        public string UserId { get; set; }

        [Column("car_id")]
        public string CarId { get; set; }

        [Column("rental_date")]
        public DateTime PickupDate { get; set; }

        [Column("return_Date")]
        public DateTime ReturnDate { get; set; }

        [Column("total_price")]
        public decimal TotalPrice { get; set; }

        [Column("payment_status")]
        public bool status { get; set; }
    }
}
