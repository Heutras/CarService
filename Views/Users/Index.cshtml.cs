using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CarService.Data;
using CarService.Models;
using CarService.Models.ViewModel;
using CarService.Utility;

namespace CarService.Pages.Users
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public UsersListViewModel UsersListVM { get; set; } //ApplicationUser ve paging info larýn listesine ihtiyacýmýz var UsersListViewModel de ihtiyacýmýz olan modeli yapýyoruz

        public async Task<IActionResult> OnGet(int productPage=1, string searchEmail=null, string searchName=null, string searchPhone=null) //varsayýlan sayfa 1
        {
            UsersListVM = new UsersListViewModel()
            {
                ApplicationUserList = await _db.ApplicationUser.ToListAsync()
            };
            StringBuilder param = new StringBuilder();
            param.Append("/Users?productPage=:"); // : sayýyla deðiþtirilecek
            param.Append("&searchName="); // arama parametreleri eklenir
            if (searchName!=null)
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
                UsersListVM.ApplicationUserList = await _db.ApplicationUser.Where(u => u.Email.ToLower().Contains(searchEmail.ToLower())).ToListAsync();
            }
            else
            {
                if (searchName != null)
                {
                    UsersListVM.ApplicationUserList = await _db.ApplicationUser.Where(u => u.Name.ToLower().Contains(searchName.ToLower())).ToListAsync();
                }
                else
                {
                    if (searchPhone != null)
                    {
                        UsersListVM.ApplicationUserList = await _db.ApplicationUser.Where(u => u.PhoneNumber.ToLower().Contains(searchPhone.ToLower())).ToListAsync();
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
                .Skip((productPage - 1) * SD.PaginationUsersPageSize) // ilk sayfada kayýt atlamýyoruz sonraki sayfalarda sayfada gösterinlen * sayfa sayýsý-1 kadar atlýyoruz
                .Take(SD.PaginationUsersPageSize).ToList();

            ViewData["VM"] = UsersListVM;
            return Page();
            
        }


    }
}
