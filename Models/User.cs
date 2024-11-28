using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCarWeb.Models
{
    [Table("MsCustomer")]   
    
    public class User
    {
        [Key]
        [Column("Customer_id")]
        public string UserId { get; set; }

        [Column("driver_license_number")]
        [StringLength(10)]
        public string DriverLicenseNumber { get; set; }

        [Column("name")]
        [Required(ErrorMessage = "Nama Lengkap harus diisi.")]
        [Display(Name = "Nama Lengkap")]
        [StringLength(100, ErrorMessage = "Nama Lengkap tidak boleh lebih dari 100 karakter.")]
        public string NamaLengkap { get; set; }

        [Column("email")]
        [Required(ErrorMessage = "Email harus diisi.")]
        [EmailAddress(ErrorMessage = "Format Email tidak valid.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Column("password")]
        [Required]
        public string PasswordHash { get; set; }

        [Column("phone_number")]
        [Required(ErrorMessage = "Nomor Telefon harus diisi.")]
        [Phone(ErrorMessage = "Format nomor telepon tidak valid.")]
        [Display(Name = "Nomor Telefon")]
        public string NomorTelp { get; set; }

        [Column("address")]
        [Required(ErrorMessage = "Alamat harus diisi.")]
        [Display(Name = "Alamat")]
        [StringLength(200, ErrorMessage = "Alamat tidak boleh lebih dari 200 karakter.")]
        public string Alamat { get; set; }
    }
}
