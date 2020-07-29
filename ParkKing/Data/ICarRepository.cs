using ParkKing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkKing.Data
{
    public interface ICarRepository
    {
        int BayAmount { get; }

        IEnumerable<Car> GetAll();
        Car GetByBayNo(int no);

        bool Secure(Car car);
        void Release(Car car);
    }
}
