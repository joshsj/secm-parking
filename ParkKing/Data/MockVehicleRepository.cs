using ParkKing.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkKing.Data.VehicleRepository
{
    public class MockVehicleRepository : IVehicleRepository
    {
        private readonly int bayAmount = 20;
        int IVehicleRepository.BayAmount => bayAmount;

        private readonly TimeSpan otpTimeout = TimeSpan.FromSeconds(10);
        TimeSpan IVehicleRepository.OtpTimeout => otpTimeout;

        private List<Vehicle> vehicles = new List<Vehicle>
        {
            new Vehicle { BayNumber = 1, Password = "password1", PhoneNumber = "01632960858" },
            new Vehicle { BayNumber = 4, Password = "password4" },
            new Vehicle { BayNumber = 5, Password = "password5" },
        };

        IEnumerable<Vehicle> IVehicleRepository.GetAll() => vehicles;

        Vehicle IVehicleRepository.GetByBayNo(int no)
            => vehicles.SingleOrDefault(vehicle => vehicle.BayNumber == no);

        SecureResult IVehicleRepository.Secure(Vehicle vehicle)
        {
            // check bay number availble
            if (vehicles.Any(c => c.BayNumber == vehicle.BayNumber))
            {
                return SecureResult.BadBayNumber;
            }

            vehicles.Add(vehicle);
            return SecureResult.Secured;
        }

        ReleaseResult IVehicleRepository.Release(Vehicle vehicle, Authentication auth)
        {
            var vehicleToRelease = vehicles.SingleOrDefault(c => c.BayNumber == vehicle.BayNumber);

            // check found
            if (vehicleToRelease == default(Vehicle))
            {
                return ReleaseResult.BadBayNumber;
            }

            // authenticate
            if (auth == Authentication.Password)
            {
                // TODO secure
                if (vehicleToRelease.Password != vehicle.Password)
                {
                    return ReleaseResult.BadPassword;
                }
            }
            else if (auth == Authentication.Otp)
            {
                if (!(vehicleToRelease.Otp?.Passcode == vehicle.Otp?.Passcode && // different passwords
                    vehicleToRelease.Otp?.TimeGenerated == vehicle.Otp?.TimeGenerated &&
                    vehicleToRelease.Otp?.TimeGenerated.Add(otpTimeout) <= DateTime.Now)  // timed out
                )
                {
                    return ReleaseResult.BadOtp;
                }
            }

            vehicles.Remove(vehicleToRelease);
            return ReleaseResult.Released;
        }

        bool IVehicleRepository.IsBayAvailable(int bayNo)
            => !vehicles.Any(vehicle => vehicle.BayNumber == bayNo) &&  // not taken
                bayNo <= bayAmount; // within max bay numbers

        GenerateOtpResult IVehicleRepository.GenerateOtp(int bayNo)
        {
            var vehicle = vehicles.SingleOrDefault(c => c.BayNumber == bayNo);

            // check found
            if (vehicle == default(Vehicle))
            {
                return GenerateOtpResult.BadBayNumber;
            }

            // check phone number provided
            if (vehicle.PhoneNumber == null)
            {
                return GenerateOtpResult.NoPhoneNumber;
            }

            // generate passcode
            vehicle.Otp = new Otp { Passcode = "000000", TimeGenerated = DateTime.Now };
            return GenerateOtpResult.Generated;
        }
    }
}
