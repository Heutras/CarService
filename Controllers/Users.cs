using CarService.Data;
using CarService.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarService.Utility;
using System.Text;
using CarService.Models;

namespace CarService.Controllers
{
    public class Users : Controller
    {

       
        public IActionResult Index()
        {
            return View();
        }
    
        public IActionResult Delete()
        {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }
    }
}
