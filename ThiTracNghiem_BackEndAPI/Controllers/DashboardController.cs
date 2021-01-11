using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThiTracNghiem_BackEndAPI.Services.ExamServices;
using ThiTracNghiem_BackEndAPI.Services.QuestionServices;
using ThiTracNghiem_BackEndAPI.Services.ScoreService;
using ThiTracNghiem_BackEndAPI.Services.UserServices;

namespace ThiTracNghiem_BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IScoreService _scoreService;
        private readonly IUserService _userService;
        private readonly IExamService _examService;
        private readonly IQuestionService _questionService;
 
        public DashboardController(IScoreService scoreService, IUserService userService, IExamService examService, IQuestionService questionService)
        {
            _scoreService = scoreService;
            _userService = userService;
            _examService = examService;
            _questionService = questionService;
        }
        [HttpGet("getInfo")]
        public async Task<IActionResult> GetInfo()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var topFive = _scoreService.GetTopFiveExamAsync();
            var userCount = _userService.GetUserCount();
            var questionCount = _questionService.GetQuestionCount();
            var examCount = _examService.GetExamCount();
            var scoreCount = _scoreService.GetScoreCount();
            var questionModule = _questionService.GetQuestionByModule();
            Task resultTask = Task.WhenAll(topFive, userCount, questionCount, examCount, scoreCount, questionModule);
            resultTask.Wait();
            var result = new
            {
                topFiveData = topFive.Result,
                userCountData = userCount.Result,
                questionCountData = questionCount.Result,
                examCountData = examCount.Result,
                scoreCountData = scoreCount.Result,
                questionModule = questionModule.Result
            };
            return Ok(result);
        }
    }
}
