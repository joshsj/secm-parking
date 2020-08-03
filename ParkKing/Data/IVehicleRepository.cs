using System;
using System.Collections.Generic;
using ParkKing.Models;

namespace ParkKing.Data.VehicleRepository
{
    public interface IVehicleRepository
    {
        int BayAmount { get; }
        TimeSpan OtpTimeout { get; }

        SecureResult Secure(Vehicle vehicle);
        ReleaseResult Release(Vehicle vehicle, Authentication auth);

        GenerateOtpResult GenerateOtp(int bayNo);

        // helpers
        bool IsBayAvailable(int bayNo);
    }

    public enum SecureResult
    {
        Secured, 
        BadBayNumber
    }

    public enum ReleaseResult
    {
        Released,
        BadBayNumber, 
        BadPassword,
        BadOtp,
    }

    public enum GenerateOtpResult
    {
        Generated,
        BadBayNumber,
        NoPhoneNumber
    }

    public enum Authentication
    {
        Password,
        Otp
    }
}
