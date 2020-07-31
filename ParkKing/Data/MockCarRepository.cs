using ParkKing.Models;
using System.Collections.Generic;
using System.Linq;

namespace ParkKing.Data.CarRepository
{
    public class MockCarRepository : ICarRepository
    {
        int ICarRepository.BayAmount => 20;

        private List<Car> cars = new List<Car>
        {
            new Car { BayNumber = 1, Password = "password1" },
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

        ReleaseResult ICarRepository.Release(Car car)
        {
            var carToRelase = cars.SingleOrDefault(c => c.BayNumber == car.BayNumber);
            
            // check found
            if (carToRelase == default(Car))
            {
                return ReleaseResult.BadBayNumber;
            }

            // check password
            // TODO secure
            if (carToRelase.Password != car.Password)
            {
                return ReleaseResult.BadPassword;
            }

            cars.Remove(carToRelase);
            return ReleaseResult.Released;
        }

        bool ICarRepository.IsBayAvailable(int bayNo)
            => !cars.Any(car => car.BayNumber == bayNo);
    }
}
