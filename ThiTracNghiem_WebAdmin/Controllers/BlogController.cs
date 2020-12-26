using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThiTracNghiem_WebAdmin.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Posts()
        {
            return View();
        }
   
        public IActionResult Categories()
        {
            return View();
        }
        public IActionResult SinglePost()
        {
            return View();
        }
         public IActionResult WritePost()
        {
            return View();
        }
    }
}
