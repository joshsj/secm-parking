using System;
using System.Collections.Generic;
using ParkKing.Models;

namespace ParkKing.Data.CarRepository
{
    public interface ICarRepository
    {
        int BayAmount { get; }
        TimeSpan OtpTimeout { get; }

        IEnumerable<Car> GetAll();
        Car GetByBayNo(int no);

        SecureResult Secure(Car car);
        ReleaseResult Release(Car car, Authentication auth);

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
