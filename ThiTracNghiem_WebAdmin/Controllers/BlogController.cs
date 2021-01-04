using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThiTracNghiem_WebAdmin.Controllers
{
    public class BlogController : BaseController
    {
        public BlogController(IConfiguration configuration) : base(configuration)
        {
        }
        public IActionResult Posts()
        {
            ViewBag.Title = "Bài viết";
            return View();
        }
   
        public IActionResult Categories()
        {
            ViewBag.Title = "Danh mục";
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
