using CarService.Data;
using CarService.Models;
using CarService.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CarService.Controllers
{
    public class Cars : Controller
    {
        public readonly ApplicationDbContext _db;
        public string StatusMessage { get; set; }

        public Cars(ApplicationDbContext db)
        {
            _db = db;
        }
        public Car Car { get; set; }

        public CarAndCustomerViewModel CarAndCustVM { get; set; }
        public IActionResult Index(string userId = null)
        {
            /*if (userId == null)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                userId = claim.Value;
            }*/

            CarAndCustVM = new CarAndCustomerViewModel()
            {
                Cars = _db.Car.Where(c => c.UserId == userId).ToList(),
                UserObj = _db.ApplicationUser.FirstOrDefault(u => u.Id == userId)
            };

            return View(model: CarAndCustVM);
        }

        //Get - Create
        public IActionResult Create(string userId = null)
        {
            Car = new Car();
            if (userId == null)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                userId = claim.Value;
            }
            Car.UserId = userId;
            return View(model: Car);
        }

        //Post - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Car obj)
        {
            _db.Car.Add(obj);
            _db.SaveChanges();
            StatusMessage = "Araç Eklendi.";
            return RedirectToAction("Index");
        }
 
    }
}

