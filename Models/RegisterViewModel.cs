using System.ComponentModel.DataAnnotations;

namespace RentCarWeb.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Nama Lengkap harus diisi.")]
        public string NamaLengkap { get; set; }

        [Required(ErrorMessage = "Email harus diisi.")]
        [EmailAddress(ErrorMessage = "Format Email tidak valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password harus diisi.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password harus memiliki minimal 6 karakter.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Re-Password harus diisi.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password dan Re-Password tidak cocok.")]
        public string RePassword { get; set; }

        [Required(ErrorMessage = "Nomor Telefon harus diisi.")]
        [Phone(ErrorMessage = "Format nomor telepon tidak valid.")]
        public string NomorTelp { get; set; }

        [Required(ErrorMessage = "Alamat harus diisi.")]
        public string Alamat { get; set; }
    }
}
