using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThiTracNghiem_BackEndAPI.Services.CategoriesServices;
using ThiTracNghiem_ViewModel.Blog;
using ThiTracNghiem_ViewModel.Commons;

namespace ThiTracNghiem_BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesServie _categoriesServies;
        public CategoriesController(ICategoriesServie categoriesServies)
        {
            _categoriesServies = categoriesServies;
        }
        [HttpGet("GetById/{categoriesId}")]
        public async Task<IActionResult> GetById(int categoriesId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _categoriesServies.GetById(categoriesId);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CategoriesRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _categoriesServies.Create(request);
            return Ok(result);
        }

        [HttpPost("update/{categoriesId}")]
        public async Task<IActionResult> Update([FromForm] CategoriesRequest request, int categoriesId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _categoriesServies.Update(request, categoriesId);
            return Ok(result);
        }
        [HttpGet("getListCategories")]
        public async Task<IActionResult> getListCategories()
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
            var result = await _categoriesServies.GetListCategories(requestBase);
            return Ok(result);
        }
        [HttpGet("delete/{categoriesId}")]
        public async Task<IActionResult> Delete([FromRoute] int categoriesId)
        {
            var result = await _categoriesServies.Delete(categoriesId);
            if (result.IsSuccessed == false) return Ok(result);
            return Ok(result);
        }
    }
}
