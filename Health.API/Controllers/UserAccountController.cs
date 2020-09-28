using Health.API.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Health.API.Controllers
{
    [ApiController]
    [Route("account/[controller]")]

    public class UserAccountController : ControllerBase
    {
        [HttpPost]
        [Route("signin")]
        [AllowAnonymous]
        public IActionResult Authenticate(UserIdentity userIdentity)
        {
            if (!PortalUser.Valid(userIdentity.LoginId, userIdentity.Passcode))
            {
                return Unauthorized("Crendential supplied not found.");
            }
            PortalUser user = PortalUser.Get(userIdentity.LoginId);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.LoginId),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(3),
                IsPersistent = false,
            };

            HttpContext.SignInAsync(
                 CookieAuthenticationDefaults.AuthenticationScheme,
                 new ClaimsPrincipal(claimsIdentity),
                 authProperties).Wait();

            return Ok("Authenticated");
        }

        [HttpPost]
        [Route("signout")]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            return Ok("Goodbye!");
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("POC: CORS cookie based authetication");
        }

        [HttpGet]
        [Route("unauthorized")]
        public IActionResult AccessDenied()
        {
            return Unauthorized("You are not allowed to access without providing proper authentication");
        }
    }
}
