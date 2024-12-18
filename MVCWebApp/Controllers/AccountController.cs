﻿using MVCWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;

namespace MVCWebApp.Controllers
{
    public class AccountController : Controller
    {
        // login and logout actions

        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;
        private RoleManager<ApplicationRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Required][EmailAddress] string email, [Required] string password, string? returnurl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser appUser = await userManager.FindByEmailAsync(email);

                if (appUser != null)
                {
                    await signInManager.SignOutAsync();
                    var role = await userManager.GetRolesAsync(appUser);

                    // Add a TraceListener to the Trace output
                    Trace.Listeners.Add(new TextWriterTraceListener(System.Console.Out));  // Writes to the console
                    Trace.AutoFlush = true;  // Ensures it writes immediately
                    // Role is coming back as nothing how weird
                    Trace.WriteLine($"Roles for user {appUser.UserName}: {string.Join(", ", appUser.CurrentRole)}");

                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(appUser, password, false, false);
                    if (result.Succeeded)
                    {
                        return Redirect(returnurl ?? "/");
                    }
                }
                ModelState.AddModelError(nameof(email), "Login Failed: Invalid Email or Password");
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Route("Account/AccessDenied")]
        public ActionResult AccessDenied()
        {
           return View();
        }

    }
}
