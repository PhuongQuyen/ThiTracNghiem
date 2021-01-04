using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThiTracNghiem_BackEndAPI.Services.ModuleService;
using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.Exams;

namespace ThiTracNghiem_BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleService _moduleService;
        public ModuleController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }
        [HttpGet("GetById/{moduleID}")]
        public async Task<IActionResult> GetById(int moduleID)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _moduleService.GetById(moduleID);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] ModuleRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _moduleService.Create(request);
            return Ok(result);
        }

        [HttpPost("update/{moduleID}")]
        public async Task<IActionResult> Update([FromForm] ModuleRequest request, int moduleID)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _moduleService.Update(request, moduleID);
            return Ok(result);
        }
        [HttpGet("getListModule")]
        public async Task<IActionResult> getListModule()
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
            var result = await _moduleService.GetListModule(requestBase);
            return Ok(result);
        }
        [HttpGet("delete/{moduleID}")]
        public async Task<IActionResult> Delete([FromRoute] int moduleID)
        {
            var result = await _moduleService.Delete(moduleID);
            if (result.IsSuccessed == false) return Ok(result);
            return Ok(result);
        }
    }
}
