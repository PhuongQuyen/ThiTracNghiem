using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThiTracNghiem_BackEndAPI.Services.RoleServices;
using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.Roles;

namespace ThiTracNghiem_BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("GetById/{roleId}")]
        public async Task<IActionResult> GetById(int roleId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _roleService.GetById(roleId);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] RoleViewModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _roleService.Create(request);
            return Ok(result);

        }

        [HttpPost("update/{roleId}")]
        public async Task<IActionResult> Update([FromForm] RoleViewModel request, int roleId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _roleService.Update(request, roleId);
            return Ok(result);
        }

        [HttpGet("delete/{roleId}")]
        public async Task<IActionResult> Delete([FromRoute] int roleId)
        {
            var result = await _roleService.Delete(roleId);
            if (result.IsSuccessed == false) return Ok(result);
            return Ok(result);
        }

        [HttpGet("getListRole")]
        public async Task<IActionResult> GetListUser()
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
            var result = await _roleService.GetListRole(requestBase);
            return Ok(result);
        }

    }
}
