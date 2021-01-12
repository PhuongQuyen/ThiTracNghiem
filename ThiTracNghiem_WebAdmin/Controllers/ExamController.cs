using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThiTracNghiem_WebAdmin.Controllers
{
    [Authorize(Roles = "3,2")]
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
       
        public IActionResult SingleExam(int examId)
        {
            ViewBag.ExamId = examId;
            return View();
        }
        [AllowAnonymous]
        public IActionResult Download()
        {
            return File("/data/QuestionTemplate.xlsx", System.Net.Mime.MediaTypeNames.Application.Octet,"QuestionTemplate.xlsx") ;
        }
        
    }
}
