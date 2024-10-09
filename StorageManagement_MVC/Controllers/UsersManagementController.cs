using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StorageManagement_MVC.Data;
using StorageManagement_MVC.Models;
using StorageManagement_MVC.ViewModels;
using System.Data;

namespace StorageManagement_MVC.Controllers
{
    public class UsersManagementController : Controller
    {

        private readonly StorageManagement_MVCContext _context;

        public UsersManagementController(StorageManagement_MVCContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Index(string SearchString)
        {
            var ListUsers = from u in _context.User select u;
            if (!String.IsNullOrEmpty(SearchString))
            {
                ListUsers = ListUsers.Where(s =>
                    s.userId!.Contains(SearchString) ||
                    s.userName!.Contains(SearchString));
            }
            return View(await ListUsers.ToListAsync());
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("userId,passWord,userName,Gender,Birthday,phoneNumber,Role")] User user)
        {
            var ListUsers = _context.User.Select(u => u.userId).ToList();
            if (ModelState.IsValid && !ListUsers.Contains(user.userId))
            {
                user.Status = 1;
                _context.User.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("userId", "User already exists");
            return View(user);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Lock_Unlock(string userId)
        {
            var user = _context.User.SingleOrDefault(u => u.userId == userId);
            if (user != null)
            {
                user.Status = user.Status == 1 ? 0 : 1;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
