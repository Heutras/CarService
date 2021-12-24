using CarService.Data;
using CarService.Models;
using CarService.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarService.Controllers
{
    public class Services : Controller
    {
        //[Authorize(Roles = SD.AdminEndUser)]
        public readonly ApplicationDbContext _db;
        public Services(ApplicationDbContext db)
        {
            _db = db;
        }
        public CarServiceViewModel CarServiceVM { get; set; }
        public ServiceHeader serviceHeader { get; set; }
        public List<ServiceHeader> ServiceHeader { get; set; }
        public List<ServiceDetails> serviceDetails { get; set; }
        public string UserId { get; set; }

        //Get - Create
        public IActionResult Create(int carId)
        {
            CarServiceVM = new CarServiceViewModel
            {
                Car = _db.Car.Include(c => c.ApplicationUser).FirstOrDefault(c => c.Id == carId),
                ServiceHeader = new Models.ServiceHeader()

            };

            List<String> lstServiceTypeInShoppingCart = _db.ServiceShoppingCart.AsQueryable()
                                                            .Include(c => c.ServiceType)
                                                            .Where(c => c.CarId == carId)
                                                            .Select(c => c.ServiceType.Name)
                                                            .ToList();

            IQueryable<Models.ServiceType> lstService = (from s in _db.ServiceType
                                                         where !(lstServiceTypeInShoppingCart.Contains(s.Name))
                                                         select s);

            CarServiceVM.ServiceTypesList = lstService.ToList();

            CarServiceVM.ServiceShoppingCart = _db.ServiceShoppingCart.Include(c => c.ServiceType).Where(c => c.CarId == carId).ToList();
            CarServiceVM.ServiceHeader.TotalPrice = 0;

            foreach (var item in CarServiceVM.ServiceShoppingCart)
            {
                CarServiceVM.ServiceHeader.TotalPrice += item.ServiceType.Price;
            }
            return View(CarServiceVM);
        }
        //Post - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CarServiceViewModel CarServiceVM)
        {
            if (ModelState.IsValid)
            {
                CarServiceVM.ServiceHeader.DateAdded = DateTime.Now;
                CarServiceVM.ServiceShoppingCart = _db.ServiceShoppingCart.Include(c => c.ServiceType).Where(c => c.CarId == CarServiceVM.Car.Id).ToList();
                foreach (var item in CarServiceVM.ServiceShoppingCart)
                {
                    CarServiceVM.ServiceHeader.TotalPrice += item.ServiceType.Price;
                }
                CarServiceVM.ServiceHeader.CarId = CarServiceVM.Car.Id;

                _db.ServiceHeader.Add(CarServiceVM.ServiceHeader);
                _db.SaveChanges();

                foreach (var detail in CarServiceVM.ServiceShoppingCart)
                {
                    ServiceDetails serviceDetails = new ServiceDetails
                    {
                        ServiceHeaderId = CarServiceVM.ServiceHeader.Id,
                        ServiceName = detail.ServiceType.Name,
                        ServicePrice = detail.ServiceType.Price,
                        ServiceTypeId = detail.ServiceTypeId
                    };

                    _db.ServiceDetails.Add(serviceDetails);

                }
                _db.ServiceShoppingCart.RemoveRange(CarServiceVM.ServiceShoppingCart);

                _db.SaveChanges();

                return RedirectToAction("Index", "Cars", new { @userId = CarServiceVM.Car.UserId });
            }

            return View();
        }
        public IActionResult Cart()
        {
            ServiceShoppingCart objServiceCart = new ServiceShoppingCart()
            {
                CarId = CarServiceVM.Car.Id,
                ServiceTypeId = CarServiceVM.ServiceDetails.ServiceTypeId
            };

            _db.ServiceShoppingCart.Add(objServiceCart);
            _db.SaveChanges();
            return RedirectToAction("Create", new { carId = CarServiceVM.Car.Id });
        }
        public IActionResult Cart(int serviceTypeId)
        {
            ServiceShoppingCart objServiceCart = _db.ServiceShoppingCart
                .FirstOrDefault(u => u.CarId == CarServiceVM.Car.Id && u.ServiceTypeId == serviceTypeId);

            _db.ServiceShoppingCart.Remove(objServiceCart);
            _db.SaveChanges();
            return RedirectToAction("Create", new { carId = CarServiceVM.Car.Id });
        }
        public IActionResult Details(int serviceId)
        {
            serviceHeader = _db.ServiceHeader.Include(s => s.Car).Include(s => s.Car.ApplicationUser).FirstOrDefault(s => s.Id == serviceId);
            serviceDetails = _db.ServiceDetails.Where(s => s.ServiceHeaderId == serviceId).ToList();
            return View(serviceHeader);

        }
        public IActionResult History(int carId)
        {
            ServiceHeader = _db.ServiceHeader.Include(s => s.Car).Include(c => c.Car.ApplicationUser).Where(c => c.CarId == carId).ToList();

            UserId = _db.Car.Where(u => u.Id == carId).ToList().FirstOrDefault().UserId;
            return View(ServiceHeader);
        }
    }
}
