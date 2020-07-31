using System.Collections.Generic;
using ParkKing.Models;

namespace ParkKing.Data.CarRepository
{
    public interface ICarRepository
    {
        int BayAmount { get; }

        IEnumerable<Car> GetAll();
        Car GetByBayNo(int no);

        SecureResult Secure(Car car);
        ReleaseResult Release(Car car);

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
        BadPassword
    }
}
