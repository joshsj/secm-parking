using ParkKing.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkKing.Data.VehicleRepository
{
    public class MockVehicleRepository : IVehicleRepository
    {
        private readonly int bayAmount;
        int IVehicleRepository.BayAmount => bayAmount;

        private readonly TimeSpan otpTimeout;
        TimeSpan IVehicleRepository.OtpTimeout => otpTimeout;

        public MockVehicleRepository(int bayAmount, TimeSpan otpTimeout)
        {
            this.bayAmount = bayAmount;
            this.otpTimeout = otpTimeout;
        }

        private List<SecureVehicle> vehicles = new List<SecureVehicle>
        {
            new SecureVehicle { BayNumber = 1, PasswordHash = new PasswordHash("password1"), PhoneNumber = "01632960858" },
            new SecureVehicle { BayNumber = 4, PasswordHash = new PasswordHash("password4") },
            new SecureVehicle { BayNumber = 5, PasswordHash = new PasswordHash("password5") },
        };

        SecureResult IVehicleRepository.Secure(Vehicle vehicle)
        {
            // check bay number availble
            if (vehicles.Any(c => c.BayNumber == vehicle.BayNumber))
            {
                return SecureResult.BadBayNumber;
            }

            vehicles.Add(new SecureVehicle
            {
                BayNumber = vehicle.BayNumber,
                PasswordHash = new PasswordHash(vehicle.Password),
                PhoneNumber = vehicle?.PhoneNumber,
                Otp = vehicle?.Otp,
            });
            return SecureResult.Secured;
        }

        ReleaseResult IVehicleRepository.Release(Vehicle vehicle, Authentication auth)
        {
            var vehicleToRelease = vehicles.SingleOrDefault(c => c.BayNumber == vehicle.BayNumber);

            // check found
            if (vehicleToRelease == default(SecureVehicle))
            {
                return ReleaseResult.BadBayNumber;
            }

            // authenticate
            if (auth == Authentication.Password)
            {
                // TODO secure
                if (!vehicleToRelease.PasswordHash.Verify(vehicle.Password))
                {
                    return ReleaseResult.BadPassword;
                }
            }
            else if (auth == Authentication.Otp)
            {
                if (!(vehicleToRelease.Otp?.Passcode == vehicle.Otp?.Passcode && // matching passwords
                    DateTime.Now < vehicleToRelease.Otp?.TimeGenerated.Add(otpTimeout))  // not timed out
                )
                {
                    return ReleaseResult.BadOtp;
                }
            }

            // one way or another, auth has passed
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
            if (vehicle == default(SecureVehicle))
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
