using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using ThiTracNghiem_ViewModel.Users;
using ThiTracNghiem_WebAdmin.Services.Users;

namespace ThiTracNghiem_WebAdmin.Controllers
{
    public class LoginController : Controller
    {

        private readonly IUserAPIClient _userAPIClient;
        private readonly IConfiguration _configuration;
        public LoginController(IUserAPIClient userAPIClient, IConfiguration configuration)
        {
            _userAPIClient = userAPIClient;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("index", "Login");
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest request, [FromQuery] string ReturnUrl)
        {
            if (!ModelState.IsValid) return View(ModelState);

            var result = await _userAPIClient.Authenticate(request);
            if (result.IsSuccessed == false)
            {
                TempData["message"] = result.Message;
                ModelState.AddModelError("", result.Message);
                return View();
            }

            var userPrincipal = this.ValidateToken(result.ResultObject);
            var isAdmin = userPrincipal.IsInRole("3");
            if (isAdmin == false)
            {
                TempData["message"] = "You do not have persmission";
                ModelState.AddModelError("", "You do not have persmission");
                return View();
            }
            TempData["Succes"] = "Login Succsess!";
            HttpContext.Session.SetString("Token", result.ResultObject);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal,
                    authProperties);

            if (!string.IsNullOrEmpty(ReturnUrl))
            {
                return LocalRedirect(ReturnUrl);
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;
            var Iss = _configuration["Tokens:Issuer"];
            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }
    }
}
