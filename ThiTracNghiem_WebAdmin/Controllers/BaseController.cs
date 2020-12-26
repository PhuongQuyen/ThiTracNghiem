using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ThiTracNghiem_WebAdmin.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IConfiguration _configuration;
        public BaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var section = context.HttpContext.Session.GetString("Token");
            if (section == null)
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
            }
            else
            {
                var userPrincipal = ValidateToken(section);
                ViewBag.Email = userPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
                ViewBag.Id = userPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                ViewBag.RoleId = userPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            }
            ViewBag.result = TempData["result"];
            ViewBag.IsSuccess = TempData["IsSuccess"];
            base.OnActionExecuting(context);
        }
        public ClaimsPrincipal ValidateToken(string jwtToken)
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
