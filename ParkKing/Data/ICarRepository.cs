using System.Collections.Generic;
using ParkKing.Models;

namespace ParkKing.Data
{
    public interface ICarRepository
    {
        int BayAmount { get; }

        IEnumerable<Car> GetAll();
        Car GetByBayNo(int no);

        bool Secure(Car car);
        bool Release(Car car);

        // helpers
        bool IsBayAvailable(int bayNo);
    }
}
