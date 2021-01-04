using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThiTracNghiem_WebAdmin.Controllers
{
    public class ExamController : BaseController
    {
        public ExamController(IConfiguration configuration) : base(configuration)
        {
        }
        public IActionResult Exams()
        {
            ViewBag.Title = "Đề thi";
            return View();
        }
        public IActionResult Modules()
        {
            ViewBag.Title = "Module";
            return View();
        }

        public IActionResult ExamManualCompose()
        {
            return View();
        }
        
       
        public IActionResult SingleExam()
        {
            return View();
        }
        
    }
}
