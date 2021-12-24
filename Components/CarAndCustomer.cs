using CarService.Data;
using CarService.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarService.Components
{
    public class CarAndCustomer : ViewComponent
    {

        public readonly ApplicationDbContext _db;
        public string StatusMessage { get; set; }

        public CarAndCustomer(ApplicationDbContext db)
        {
            _db = db;
        }

        public CarAndCustomerViewModel CarAndCustVM { get; set; }
        public IViewComponentResult Invoke(string userId = null)
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

            return View(CarAndCustVM);
        }
    }
}
