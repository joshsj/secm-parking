using ParkKing.Data;

namespace ParkKing.Models
{
    public class SecureVehicle
    {
        public int BayNumber { get; set; }
        public string PhoneNumber { get; set; }
        public PasswordHash PasswordHash { get; set; }
        public Otp Otp { get; set; }
    }
}
