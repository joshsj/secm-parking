using System;
using System.ComponentModel.DataAnnotations;

namespace ParkKing.Models
{
    public class Otp
    {
        [Required]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP is 6 characters.")]
        public string Passcode { get; set; }

        public DateTime TimeGenerated { get; set; }
    }
}
