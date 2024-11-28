using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentCarWeb.Models
{
    [Table("MsCarImages")]
    public class CarImage
    {
        [Key]
        [Column("Image_car_id")]
        public string image_car_id { get; set; }
        [Column("Car_id")]
        public string Car_id { get; set; }
        [Column("image_link")]
        public string image_link { get; set; }

    }
}
