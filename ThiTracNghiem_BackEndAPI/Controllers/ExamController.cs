using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using ThiTracNghiem_BackEndAPI.Services.ExamServices;
using ThiTracNghiem_BackEndAPI.Services.QuestionServices;
using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.Exams;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
//sasca
namespace ThiTracNghiem_BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;
        private readonly IQuestionService _questionService;
        public ExamController(IExamService examService, IQuestionService questionService)
        {
            _examService = examService;
            _questionService = questionService;
        }
        // GET: api/<ExamController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var start = Request.Query["start"].FirstOrDefault();
            var length = Request.Query["length"].FirstOrDefault();
            DatatableRequestBase requestBase = new DatatableRequestBase()
            {
                Draw = Request.Query["draw"].FirstOrDefault(),
                Skip = start != null ? Convert.ToInt32(start) : 0,
                PageSize = length != null ? Convert.ToInt32(length) : 0,
                sortColumn = Request.Query["order[0][column]"].FirstOrDefault(),
                sortColumnDirection = Request.Query["order[0][dir]"].FirstOrDefault(),
                searchValue = Request.Query["search[value]"].FirstOrDefault()
            };

            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _examService.GetListExam(requestBase);
            return Ok(result);
        }

        // GET api/<ExamController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _examService.GetById(id);
            return Ok(result);
        }

        // POST api/<ExamController>
        [HttpPost]
        public IActionResult Post([FromForm] ExamViewModel exam)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var status = _examService.Create(exam);
            return Ok(status);
        }

        // PUT api/<ExamController>/5
        [HttpPost("update/{id}")]
        public async Task<IActionResult> Update(int id,[FromForm] ExamViewModel examViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var status = await _examService.Update(examViewModel, id);
            return Ok(status);
        }

        // DELETE api/<ExamController>/5
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _examService.Delete(id);
            return Ok(result);
        }

        [HttpGet("GetListQuestion/{examId}")]
        public async Task<IActionResult> GetListQuestion(int examId)
        {
            var start = Request.Query["start"].FirstOrDefault();
            var length = Request.Query["length"].FirstOrDefault();
            DatatableRequestBase requestBase = new DatatableRequestBase()
            {
                Draw = Request.Query["draw"].FirstOrDefault(),
                Skip = start != null ? Convert.ToInt32(start) : 0,
                PageSize = length != null ? Convert.ToInt32(length) : 0,
                sortColumn = Request.Query["order[0][column]"].FirstOrDefault(),
                sortColumnDirection = Request.Query["order[0][dir]"].FirstOrDefault(),
                searchValue = Request.Query["search[value]"].FirstOrDefault()
            };
            var result = await _questionService.GetListQuestionByExam(requestBase, examId);
            return Ok(result);
        }

        [HttpGet("GetInfoExam/{roomId}")]
        public async Task<IActionResult> GetInfoExam(int roomId)
        {
            var result = await _examService.GetByRoomId(roomId);
            return Ok(result);
        }
     }
}
