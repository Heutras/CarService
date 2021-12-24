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
        public readonly ApplicationDbContext _db;

        public Users(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        public UsersListViewModel UsersListVM { get; set; }

        public IActionResult Index(int productPage = 1, string searchEmail = null, string searchName = null, string searchPhone = null)
        {
            UsersListVM = new UsersListViewModel()
            {
                ApplicationUserList =  _db.ApplicationUser.ToList()
            };
            StringBuilder param = new StringBuilder();
            param.Append("/Users?productPage=:"); // : sayıyla değiştirilecek
            param.Append("&searchName="); // arama parametreleri eklenir
            if (searchName != null)
            {
                param.Append(searchName);
            }
            param.Append("&searchEmail=");
            if (searchEmail != null)
            {
                param.Append(searchEmail);
            }
            param.Append("&searchPhone=");
            if (searchPhone != null)
            {
                param.Append(searchPhone);
            }

            if (searchEmail != null)
            {
                UsersListVM.ApplicationUserList =  _db.ApplicationUser.Where(u => u.Email.ToLower().Contains(searchEmail.ToLower())).ToList();
            }
            else
            {
                if (searchName != null)
                {
                    UsersListVM.ApplicationUserList =  _db.ApplicationUser.Where(u => u.Name.ToLower().Contains(searchName.ToLower())).ToList();
                }
                else
                {
                    if (searchPhone != null)
                    {
                        UsersListVM.ApplicationUserList =  _db.ApplicationUser.Where(u => u.PhoneNumber.ToLower().Contains(searchPhone.ToLower())).ToList();
                    }
                }
            }

            var count = UsersListVM.ApplicationUserList.Count;

            UsersListVM.PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = SD.PaginationUsersPageSize,
                TotalItems = count,
                UrlParam = param.ToString()
            };

            UsersListVM.ApplicationUserList = UsersListVM.ApplicationUserList.OrderBy(p => p.Email)
                .Skip((productPage - 1) * SD.PaginationUsersPageSize) // ilk sayfada kayıt atlamıyoruz sonraki sayfalarda sayfada gösterinlen * sayfa sayısı-1 kadar atlıyoruz
                .Take(SD.PaginationUsersPageSize).ToList();

            return View(UsersListVM);
        }
    
        public IActionResult Delete(string id)
        {
            if (id.Trim().Length == 0)
            {
                return NotFound();
            }

            ApplicationUser =  _db.ApplicationUser.FirstOrDefault(m => m.Id == id);

            if (ApplicationUser == null)
            {
                return NotFound();
            }
            return View();
        }
        public IActionResult Delete()
        {
            var userInDb =  _db.Users.SingleOrDefault(u => u.Id == ApplicationUser.Id);

            _db.Users.Remove(userInDb);
             _db.SaveChanges();

            return RedirectToPage("Index");
        }
        public IActionResult Edit(string id)
        {
            if (id.Trim().Length == 0)
            {
                return NotFound();
            }

            ApplicationUser =  _db.ApplicationUser.FirstOrDefault(m => m.Id == id);

            if (ApplicationUser == null)
            {
                return NotFound();
            }
            return View();
        }
        public IActionResult Edit()
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                var userInDb =  _db.ApplicationUser.SingleOrDefault(u => u.Id == ApplicationUser.Id);
                if (userInDb == null)
                {
                    return NotFound();
                }
                else
                {
                    userInDb.Name = ApplicationUser.Name;
                    userInDb.PhoneNumber = ApplicationUser.PhoneNumber;
                    userInDb.Address = ApplicationUser.Address;
                    userInDb.City = ApplicationUser.City;
                    userInDb.PostalCode = ApplicationUser.PostalCode;

                     _db.SaveChanges();
                    return RedirectToPage("Index");
                }
            }
        
        }
    }
}
