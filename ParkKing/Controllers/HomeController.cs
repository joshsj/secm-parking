﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ParkKing.ViewModels;
using System.Linq;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using ParkKing.Data.VehicleRepository;

namespace ParkKing.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
