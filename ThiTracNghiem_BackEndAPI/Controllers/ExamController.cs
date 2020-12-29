﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using ThiTracNghiem_BackEndAPI.Services.ExamServices;
using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.Exams;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
//sasca
namespace ThiTracNghiem_BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;
        public ExamController(IExamService examService)
        {
            _examService = examService;
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
        public IActionResult Post([FromBody] ExamViewModel exam)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var status = _examService.Create(exam);
            return Ok(status);
        }

        // PUT api/<ExamController>/5
        [HttpPost("update/{id}")]
        public async Task<IActionResult> Update(int id,[FromBody] ExamViewModel examViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var status = await _examService.Update(examViewModel, id);
            return Ok(status);
        }

        // DELETE api/<ExamController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            return;
        }
    }
}
