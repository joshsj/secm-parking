using ParkKing.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkKing.Data.CarRepository
{
    public class MockCarRepository : ICarRepository
    {
        private readonly int bayAmount = 20;
        int ICarRepository.BayAmount => bayAmount;

        private readonly TimeSpan otpTimeout = TimeSpan.FromSeconds(10);
        TimeSpan ICarRepository.OtpTimeout => otpTimeout;

        private List<Car> cars = new List<Car>
        {
            new Car { BayNumber = 1, Password = "password1", PhoneNumber = "01632960858" },
            new Car { BayNumber = 4, Password = "password4" },
            new Car { BayNumber = 5, Password = "password5" },
        };

        IEnumerable<Car> ICarRepository.GetAll() => cars;

        Car ICarRepository.GetByBayNo(int no)
            => cars.SingleOrDefault(car => car.BayNumber == no);

        SecureResult ICarRepository.Secure(Car car)
        {
            // check bay number availble
            if (cars.Any(c => c.BayNumber == car.BayNumber))
            {
                return SecureResult.BadBayNumber;
            }

            cars.Add(car);
            return SecureResult.Secured;
        }

        ReleaseResult ICarRepository.Release(Car car, Authentication auth)
        {
            var carToRelase = cars.SingleOrDefault(c => c.BayNumber == car.BayNumber);

            // check found
            if (carToRelase == default(Car))
            {
                return ReleaseResult.BadBayNumber;
            }

            // authenticate
            if (auth == Authentication.Password)
            {
                // TODO secure
                if (carToRelase.Password != car.Password)
                {
                    return ReleaseResult.BadPassword;
                }
            }
            else if (auth == Authentication.Otp)
            {
                if (!(carToRelase.Otp?.Passcode == car.Otp?.Passcode && // different passwords
                    carToRelase.Otp?.TimeGenerated == car.Otp?.TimeGenerated &&
                    carToRelase.Otp?.TimeGenerated.Add(otpTimeout) <= DateTime.Now)  // timed out
                )
                {
                    return ReleaseResult.BadOtp;
                }
            }

            cars.Remove(carToRelase);
            return ReleaseResult.Released;
        }

        bool ICarRepository.IsBayAvailable(int bayNo)
            => !cars.Any(car => car.BayNumber == bayNo) &&  // not taken
                bayNo <= bayAmount; // within max bay numbers

        GenerateOtpResult ICarRepository.GenerateOtp(int bayNo)
        {
            // get car
            var car = cars.SingleOrDefault(c => c.BayNumber == bayNo);

            // check found
            if (car == default(Car))
            {
                return GenerateOtpResult.BadBayNumber;
            }

            // check phone number provided
            if (car.PhoneNumber == null)
            {
                return GenerateOtpResult.NoPhoneNumber;
            }

            // generate passcode
            car.Otp = new Otp { Passcode = "000000", TimeGenerated = DateTime.Now };
            return GenerateOtpResult.Generated;
        }
    }
}
