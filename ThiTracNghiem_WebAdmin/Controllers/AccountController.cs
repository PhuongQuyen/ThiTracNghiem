using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ThiTracNghiem_WebAdmin.Services.Users;

namespace ThiTracNghiem_WebAdmin.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserAPIClient _userAPIClient;
        private readonly IConfiguration _configuration;
        public AccountController(IUserAPIClient userAPIClient, IConfiguration configuration)
        {
            _userAPIClient = userAPIClient;
            _configuration = configuration;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var result = await _userAPIClient.getListUser();
            ViewData["users"] = result.ResultObject;
            return View();
        }
    }
}
