using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThiTracNghiem_WebAdmin.Controllers
{
    public class RoomController : Controller
    {
        public IActionResult ExamInRoom() {
            ViewBag.Title = "Đề thi";
            return View();
        }
        public IActionResult Rooms()
        {
            return View();
        }


    }
}
