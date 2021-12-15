using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarService.Controllers
{
    public class ServiceType : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
