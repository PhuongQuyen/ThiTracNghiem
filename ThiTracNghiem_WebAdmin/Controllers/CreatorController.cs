using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ThiTracNghiem_WebAdmin.Controllers
{
    [Authorize(Roles = "4")]
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
        public IActionResult ExamInRoom(int roomId)
        {
            ViewBag.RoomId = roomId;
            return View();
        }
        public IActionResult StudentInRoom(int roomId)
        {
            ViewBag.RoomId = roomId;
            return View();
        }
    }
}
