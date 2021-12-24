using CarService.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarService.Controllers
{
    public class ServiceType : Controller
    {
        public readonly ApplicationDbContext _db;

        public ServiceType(ApplicationDbContext db)
        {
            _db = db;
        }
        public IList<ServiceType> Servicetype { get; set; }
        public Models.ServiceType servicetype { get; set; }


        public IActionResult Index()
        {
            Servicetype = (IList<ServiceType>)_db.ServiceType.ToList();
            return View(Servicetype);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            servicetype  = _db.ServiceType.FirstOrDefault( m => m.Id == id);

            if (servicetype == null)
            {
                return NotFound();
            }
            return View();
        }
        public IActionResult Edit(Models.ServiceType servicetype)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var serviceFromDb =  _db.ServiceType.FirstOrDefault(s => s.Id == servicetype.Id);
            serviceFromDb.Name = servicetype.Name;
            serviceFromDb.Price = servicetype.Price;
             _db.SaveChanges();

            return RedirectToAction("Index");
            
        }
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            servicetype =  _db.ServiceType.FirstOrDefault(m => m.Id == id);

            if (servicetype == null)
            {
                return NotFound();
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            servicetype =  _db.ServiceType.FirstOrDefault(m => m.Id == id);

            if (servicetype == null)
            {
                return NotFound();
            }
            return View();
        }
        public IActionResult Delete(Models.ServiceType servicetype)
        {
            if (servicetype == null)
            {
                return NotFound();
            }

            _db.ServiceType.Remove(servicetype);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Create(Models.ServiceType servicetype)
        {
            if (!ModelState.IsValid)
            {
                return View(); //istenilen alanlar gönderilmezse uyarı verir
            }

            _db.ServiceType.Add(servicetype);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
