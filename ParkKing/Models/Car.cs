using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParkKing.Models
{
    public class Car
    {
        [Required(ErrorMessage = "Bay number is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Bay number must be greater than zero.")]
        public int BayNumber { get; set; }

        [Required (ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be 8 letters or more.")]
        public string Password { get; set; }
    }
}
