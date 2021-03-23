using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartOffice.Data;
using SmartOffice.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace SmartOffice.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ViewResult Index()
        {
            ClaimsPrincipal currentUser = this.User;
            string idOfCurrentUser = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            string roleOfCurrentUser = currentUser.FindFirst(ClaimTypes.Role).Value;

            var allUsers = _context.Users
                .Select(s => new {
                    Names = s.FirstName + " " + s.LastName,
                    Id = s.Id
                })
                .OrderBy(x=> x.Names)
                .ToList();

            ViewBag.AllUsers = allUsers;
            return View();
        }
    }
}
