using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThiTracNghiem_WebAdmin.Controllers
{
    public class QuestionController : Controller
    {
        public IActionResult Questions()
        {
            return View();
        }
    }
}
