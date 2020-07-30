using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ParkKing.Configuration;
using ParkKing.Data;
using ParkKing.Models;
using ParkKing.ViewModels.YourCar;

namespace ParkKing.Controllers
{   
    public class YourCarController : Controller
    {
        private readonly ICarRepository carRepo;
        private readonly string bayNoSessionKey;

        public YourCarController(IOptions<SessionConfiguration> options, ICarRepository carRepo)
        {
            this.carRepo = carRepo;
            bayNoSessionKey = options.Value.BayNoSessionKey;
        }

        public IActionResult Index()
        {
            // check for sesssion 
            var clientBayNo = HttpContext.Session.GetInt32(bayNoSessionKey);
            if (!clientBayNo.HasValue) // not parked
            {
                // redirect to register page
                return RedirectToAction(nameof(Register));
            }

            // get client car
            var car = carRepo.GetByBayNo(clientBayNo.Value);
            return View(car);
        }

        [HttpGet]
        public IActionResult Register()
        {
            // check for sesssion 
            var bayNo = HttpContext.Session.GetInt32(bayNoSessionKey);
            if (bayNo.HasValue) // check parked
            {
                // redirect to register page
                return RedirectToAction(nameof(Index));
            }

            return View(new Car());
        }

        [HttpPost]
        public IActionResult Register(Car car)
        {
            if (!carRepo.Secure(car))
            {
                return View(car);
            }

            // store bay id in session
            HttpContext.Session.SetInt32(bayNoSessionKey, car.BayNumber);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Release()
        {
            var bayNo = HttpContext.Session.GetInt32(bayNoSessionKey);
            var car = carRepo.GetByBayNo(bayNo.Value);

            carRepo.Release(car);

            // remove session value
            HttpContext.Session.Remove(bayNoSessionKey);

            return Redirect("/"); // go home
        }
    }
}