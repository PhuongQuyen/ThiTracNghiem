using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ThiTracNghiem_BackEndAPI.Services.ExamServices;
using ThiTracNghiem_BackEndAPI.Services.QuestionServices;
using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.Exams;
using ThiTracNghiem_ViewModel.Questions;
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

        [HttpPost("uploadExcel/{examId}")]
        public async Task<IActionResult> UploadFileExcel(int examId,IFormFile myfile)
        {
            var file = HttpContext.Request.Form.Files[0];
            var filePath = Path.GetFileName(file.FileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
                // If you are a commercial business and have
                // purchased commercial licenses use the static property
                // LicenseContext of the ExcelPackage class:
                ExcelPackage.LicenseContext = LicenseContext.Commercial;

                // If you use EPPlus in a noncommercial context
                // according to the Polyform Noncommercial license:
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    // get number of rows and columns in the sheet
                    int rows = worksheet.Dimension.End.Row; // 20
                    int columns = worksheet.Dimension.End.Column; // 7

                    // loop through the worksheet rows and columns
                    for (int i =3; i <= rows; i++)
                    {
                        if (worksheet.Cells[i, 1].Value == null) break;
                        QuestionRequest question = new QuestionRequest()
                        {
                            ExamId = examId,
                            QuestionContent = ((worksheet.Cells[i, 2].Value) ?? "").ToString(),
                            QuestionType = int.Parse((worksheet.Cells[i, 3].Value??"0").ToString()),
                            CorrectAnswers = ((worksheet.Cells[i, 4].Value) ?? "").ToString(),
                            AnswerExplain = ((worksheet.Cells[i, 5].Value) ?? "").ToString(),
                            A = ((worksheet.Cells[i, 6].Value) ?? "").ToString(),
                            B= ((worksheet.Cells[i, 7].Value) ?? "").ToString(),
                            C = ((worksheet.Cells[i, 8].Value) ?? "").ToString(),
                            D= ((worksheet.Cells[i, 9].Value) ?? "").ToString(),
                        };
                       await _questionService.Create(question);
                    }
                }
                
            }
            return Ok(new ApiResult<String>().Message="OK");
        }
     }
}
