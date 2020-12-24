using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThiTracNghiem_BackEndAPI.Models;
using ThiTracNghiem_BackEndAPI.ViewModels;

namespace ThiTracNghiem_BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {

        private readonly tracnghiemContext _context;

        public AnswerController( tracnghiemContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var query = from l in _context.Answers
                        select l;

            var data = await query.Select(x => new AnswerViewModel()
            {
                Id = x.Id,
                QuestionId = x.QuestionId,
                A = x.A,
                B = x.B,
                C = x.C,
                D = x.D,
                CorrectAnswers = x.CorrectAnswers,
                AnswerExplain = x.AnswerExplain
            }).ToListAsync();

            return Ok(data);
        }
    }
}
