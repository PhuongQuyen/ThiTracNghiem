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
    public class QuestionController : BaseController
    {
        public QuestionController( IConfiguration configuration) : base(configuration)
        {
        }
        public IActionResult Questions()
        {
            ViewBag.Title = "Câu hỏi";
            return View();
        }
    }
}
