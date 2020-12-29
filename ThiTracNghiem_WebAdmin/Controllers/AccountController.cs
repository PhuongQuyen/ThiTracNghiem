using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ThiTracNghiem_WebAdmin.Services.Users;

namespace ThiTracNghiem_WebAdmin.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserAPIClient _userAPIClient;
        public AccountController(IUserAPIClient userAPIClient, IConfiguration configuration):base(configuration)
        {
            _userAPIClient = userAPIClient;
        }
        public IActionResult Accounts()
        {
            ViewBag.Title = "Tài khoản";
            return View();
        }

        public IActionResult Roles()
        {
            ViewBag.Title = "Quyền";
            return View();
        }
    }
}
