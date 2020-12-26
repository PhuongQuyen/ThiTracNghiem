using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThiTracNghiem_WebAdmin.Controllers
{
    public class CreatorController : Controller
    {
        public IActionResult ExamInRoom()
        {
            return View();
        }
        public IActionResult Rooms()
        {
            return View();
        }


    }
}
