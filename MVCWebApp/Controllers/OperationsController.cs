﻿/* 30074191/Keanu Farro
 * CODE TAKEN FROM TUTORIAL:
 * https://www.yogihosting.com/aspnet-core-identity-mongodb/
 */

using MVCWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Controllers
{
    public class OperationsController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private RoleManager<ApplicationRole> roleManager;

        public OperationsController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public ViewResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser appUser = new ApplicationUser
                {
                    UserName = user.Name,
                    Email = user.Email,
                    CurrentRole = "Admin"
                };

                IdentityResult result = await userManager.CreateAsync(appUser, user.Password);

                // Adding User to Admin Role
                await userManager.AddToRoleAsync(appUser, appUser.CurrentRole);

                if (result.Succeeded)
                    ViewBag.Message = "User Created Successfully";
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }

        public IActionResult CreateRole() => View();
        [HttpPost]
        public async Task <IActionResult> CreateRole([Required] string name)
        {
            if (ModelState.IsValid)
            {
                //var role = roleManager.FindByIDAsync
                IdentityResult result = await roleManager.CreateAsync(new ApplicationRole()  { Name = name });
                if (result.Succeeded)
                    ViewBag.Message = "Role Created Successfully";
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }
    }
}
