using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThiTracNghiem_BackEndAPI.Services.UserServices;
using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.Users;

namespace ThiTracNghiem_BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var resultToken = await _userService.Authencate(request);
            if (string.IsNullOrEmpty(resultToken.ResultObject))
            {
                return Ok(resultToken);
            }
            return Ok(resultToken);
        }

        [HttpGet("GetById/{userId}")]
        public async Task<IActionResult> GetById( int userId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _userService.GetById(userId);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _userService.Register(request);
            return Ok(result);

        }

        [HttpPost("update/{userId}")]
        public async Task<IActionResult> Update([FromForm] RegisterRequest request,int userId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _userService.Update(request, userId);
            return Ok(result);
        }

        [HttpGet("delete/{userId}")]
        public async Task<IActionResult> Delete([FromRoute] int userId)
        {
            var result = await _userService.Delete(userId);
            if (result.IsSuccessed == false) return Ok(result);
            return Ok(result);
        }

        [HttpGet("getListUser")]
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
            var result = await _userService.GetListUser(requestBase);
            return Ok(result);
        }
    }
}
