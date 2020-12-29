using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ThiTracNghiem_WebAdmin.Controllers
{
    public class CreatorController : BaseController
    {
        public CreatorController(IConfiguration configuration) : base(configuration)
        {
        }
        public IActionResult Index()
        {
            ViewBag.Title = "Đề thi";
            return View();
        }
        public IActionResult ExamInRoom()
        {
            return View();
        }
    }
}
