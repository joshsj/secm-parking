using System.ComponentModel.DataAnnotations;

namespace ParkKing.Models
{
    public class Vehicle
    {
        [Required(ErrorMessage = "Bay number is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Bay number must be greater than zero.")]
        public int BayNumber { get; set; }

        [Required (ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be 8 letters or more.")]
        public string Password { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public Otp Otp { get; set; }
    }
}
