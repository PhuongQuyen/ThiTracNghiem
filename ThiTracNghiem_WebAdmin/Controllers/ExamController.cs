using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThiTracNghiem_WebAdmin.Controllers
{
    public class ExamController : Controller
    {
        
       public IActionResult Exams()
        {
            return View();
        }
        public IActionResult Modules()
        {
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
