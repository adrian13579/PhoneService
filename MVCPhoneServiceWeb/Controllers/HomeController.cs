﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MVCPhoneServiceWeb.Utils;
using Repo;

namespace MVCPhoneServiceWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        public HomeController(ApplicationDbContext context, 
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            if (!await _roleManager.RoleExistsAsync(SD.AdminUser))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.AdminUser));
            }
            if (!await _roleManager.RoleExistsAsync(SD.VisitorUser))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.VisitorUser));
            }
            //var user = new IdentityUser
            //{
            //    UserName = "Administrator@mcv.com",
            //    Email = "Administrator@mcv.com"
            //};
            //var lookingUser = await _context.Users.FirstOrDefaultAsync(m => m.Email == user.Email);
            //if (lookingUser == null)
            //{
            //    var admin = await  _userManager.CreateAsync(user, "McvAdministrator1!");
            //    if (admin.Succeeded)
            //    {
            //        await _context.SaveChangesAsync();

                   
                   
            //        //userFromDb.EmailConfirmed = true;
            //        //userFromDb.LockoutEnabled = false;
            //        await _context.SaveChangesAsync();
            //    }


            //}

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}