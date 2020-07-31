﻿using Microsoft.AspNetCore.Mvc;
using ParkKing.Data;
using ParkKing.Models;
using ParkKing.ViewModels;

namespace ParkKing.Controllers
{
    public class SecureController : Controller
    {
        private readonly ICarRepository carRepo;

        public SecureController(ICarRepository carRepo)
        {
            this.carRepo = carRepo;
        }

        public IActionResult Index()
            => View(new Car());

        [HttpPost]
        public IActionResult Index(Car car)
        {
            // check bay is available
            if (!carRepo.IsBayAvailable(car.BayNumber))
            {
                // add error to model
                ModelState.AddModelError(nameof(Car.BayNumber), "Bay is occupied by another vehicle.");
                return View(car);
            }

            return RedirectToAction(nameof(Details), car);
        }

        public IActionResult Details(Car car)
            => View(car);

        [HttpPost]
        public IActionResult Finalise(Car car)
        {
            if (!carRepo.Secure(car))
            {
                return View("Message", new MessageViewModel
                {
                    IsSuccessMessage = false,
                    Message = "There was a problem securing your vehicle."
                });
            }


           return View("Message", new MessageViewModel
           {
               IsSuccessMessage = true,
               Message = "Vehicle secured!"
           });
        }
    }
}