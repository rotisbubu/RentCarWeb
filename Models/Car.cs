using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCarWeb.Models
{
    [Table("MsCar")]
    public class Car
    {
        [Key]
        [Column("Car_id")]
        public string Car_Id { get; set; }
        [Column("name")]
        public string name { get; set; }
        [Column("model")]
        public string model { get; set; }
        [Column("year")]
        public int year { get; set; }
        [Column("license_plate")]
        public string license_plate { get; set; }
        [Column("number_of_car_seats")]
        public int number_of_car_seats { get; set; }
        [Column("transmission")]
        public string transmission { get; set; }
        [Column("price_per_day")]
        public decimal price_per_day { get; set; }
        [Column("status")]
        public bool status { get; set; }

    }
}
