using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using StorageManagement_MVC.Data;
using StorageManagement_MVC.Models;
using StorageManagement_MVC.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StorageManagement_MVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly StorageManagement_MVCContext _context;

        public UsersController(StorageManagement_MVCContext context)
        {
            _context = context;
        }

        

        [HttpGet]
        public IActionResult Login(string? ReturnUrl) 
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value == "ADMIN")
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value == "ACCOUNTING")
                {
                    return RedirectToAction("Index", "Accounting");
                }
            }
            ViewBag.ReturnUrl = ReturnUrl;          
            return View(); 
        }

        [HttpPost]

        public async Task<IActionResult> Login(LoginVM model, string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                var user = _context.User.SingleOrDefault(u => u.userId == model.Username);
                if (user == null)
                {
                    ModelState.AddModelError("Error","User does not exist");
                }
                else if (user.passWord != model.Password)
                    ModelState.AddModelError("Error", "Wrong password");
                else
                {
                    if(user.Status == 0)
                        ModelState.AddModelError("Error can't login", $"User [{user.userId}] is locked");
                    else
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.userId),
                            new Claim(ClaimTypes.Name, user.userName),
                            new Claim(ClaimTypes.MobilePhone, user.phoneNumber.ToString()),
                            new Claim(ClaimTypes.Gender, user.Gender),
                            new Claim(ClaimTypes.DateOfBirth, user.Birthday.HasValue ? user.Birthday.Value.ToString("yyyy-MM-dd") : string.Empty),
                            new Claim(ClaimTypes.Role, user.Role),
                        };
                        var claimsIdentity = new ClaimsIdentity(claims,
                            CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        if(user.Role == "ADMIN")
                        {
                            return Redirect("/home/index");
                        }
                        else if(user.Role == "ACCOUNTING")
                        {
                            return Redirect("/Accounting/index");
                        }
                    }    
                }
            }
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {

            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        [Authorize]
        public IActionResult Profile()
        {          
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Edit(string userId)
        {
            if (userId == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string userId, [Bind("Role,passWord,Status,userId,userName,Gender,Birthday,phoneNumber")] User user)
        {
            if (userId != user.userId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.userId),
                            new Claim(ClaimTypes.Name, user.userName),
                            new Claim(ClaimTypes.MobilePhone, user.phoneNumber.ToString()),
                            new Claim(ClaimTypes.Gender, user.Gender),
                            new Claim(ClaimTypes.DateOfBirth, user.Birthday.HasValue ? user.Birthday.Value.ToString("yyyy-MM-dd") : string.Empty),
                            new Claim(ClaimTypes.Role, user.Role),
                        };
                    var claimsIdentity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.userId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Profile));
            }
            return View(user);
        }


        // GET: /Account/ChangePassword
        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Account/ChangePassword
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = _context.User.SingleOrDefault(u => u.userId == userId);
            if(user.passWord != model.OldPassword)
            {
                ModelState.AddModelError("OldPassword", "Old password is incorrect.");
                return View(model);
            }

            user.passWord = model.NewPassword;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("profile");
        }

        private bool UserExists(string userId)
        {
            return (_context.User?.Any(e => e.userId == userId)).GetValueOrDefault();
        }
    }
}
