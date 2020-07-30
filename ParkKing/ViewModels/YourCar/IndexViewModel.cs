using ParkKing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkKing.ViewModels.YourCar
{
    public class IndexViewModel
    {
        public Car Car { get; set; }
        public bool HasMessage => Message != null;
        public bool MessageSuccess { get; set; }
        public string Message { get; set; }
    }
}
