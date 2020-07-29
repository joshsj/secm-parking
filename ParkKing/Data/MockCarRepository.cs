using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkKing.Models;

namespace ParkKing.Data
{
    public class MockCarRepository : ICarRepository
    {
        int ICarRepository.BayAmount => 20;

        private List<Car> cars = new List<Car>
        {
            new Car { BayNumber = 1, Password = "wasd" },
            new Car { BayNumber = 4, Password = "123" },
            new Car { BayNumber = 5, Password = "qwe" },
        };

        IEnumerable<Car> ICarRepository.GetAll() => cars;

        Car ICarRepository.GetByBayNo(int no)
            => cars.SingleOrDefault(car => car.BayNumber == no);

        bool ICarRepository.Secure(Car car)
        {
            throw new NotImplementedException();
        }

        void ICarRepository.Release(Car car)
        {
            throw new NotImplementedException();
        }
    }
}
