using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThiTracNghiem_BackEndAPI.Services.QuestionServices;
using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.Questions;

namespace ThiTracNghiem_BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet("GetById/{questionId}")]
        public async Task<IActionResult> GetById(int questionId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _questionService.GetById(questionId);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] QuestionRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _questionService.Create(request);
            return Ok(result);
        }

        [HttpPost("update/{questionId}")]
        public async Task<IActionResult> Update([FromForm] QuestionRequest request, int questionId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _questionService.Update(request, questionId);
            return Ok(result);
        }

        [HttpGet("delete/{questionId}")]
        public async Task<IActionResult> Delete([FromRoute] int questionId)
        {
            var result = await _questionService.Delete(questionId);
            if (result.IsSuccessed == false) return Ok(result);
            return Ok(result);
        }

        [HttpGet("GetListQuestion")]
        public async Task<IActionResult> GetListQuestion()
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
            var result = await _questionService.GetListQuestion(requestBase);
            return Ok(result);
        }
        //getListquestion(romid)
        [HttpGet("GetListQuestionbyromid/{roomId}")]
        public async Task<IActionResult> GetListQuestionByRomId([FromRoute] int roomId)
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
            var result = await _questionService.GetListQuestionByRoom(requestBase, roomId);
            return Ok(result);
        }

        //getListquestion(romid)
        [HttpGet("GetQuestionModuleByExamId/{examId}")]
        public async Task<IActionResult> GetQuestionModuleByExamId([FromRoute] int examId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _questionService.GetQuestionByModule(examId);
            return Ok(result);
        }

        [HttpGet("GetCountQuestion/{examId}")]
        public async Task<IActionResult> GetCountQuestion([FromRoute] int examId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _questionService.GetCountTotalQuestion(examId);
            return Ok(result);
        }

    }
}
