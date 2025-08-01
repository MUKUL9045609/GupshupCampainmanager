﻿using Gupshupcampainmanager.Models;
using Gupshupcampainmanager.Repository.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gupshupcampainmanager.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            ViewData["Navbar"] = false;
            ViewData["ShowSidebar"] = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel login) 
        {
            var command = await _authService.Login(login); 

            if (command != null && command.IsSuccess)
            {
                HttpContext.Session.SetString("UserName", command.UserName);
                HttpContext.Session.SetString("Role", command.Role);
                var userclims = new List<Claim>
                 {
                    new Claim(ClaimTypes.Name, command.UserName),
                    new Claim(ClaimTypes.Role, command.Role)
                 };
                var userIdentity = new ClaimsIdentity(userclims, "Cookie");
                var AuthProperty = new AuthenticationProperties
                {
                    IsPersistent = true,
                };
                var userPrincipal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync("Cookie", userPrincipal, AuthProperty);

                return RedirectToAction("SaveCampaignTemplate", "Gupshup");
            };
            ViewBag.Error = "Invalid username or password";
            return View();
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            await HttpContext.SignOutAsync("Cookie");
            // Clear session if needed
            HttpContext.Session.Clear();
            // Redirect to login page
            return RedirectToAction("Login", "Auth");
        }


    }
}
