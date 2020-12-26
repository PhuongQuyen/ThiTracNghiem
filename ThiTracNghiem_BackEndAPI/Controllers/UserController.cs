using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThiTracNghiem_BackEndAPI.Services.UserServices;
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

        [HttpGet("{userId}")]
        public async Task<IActionResult> Update( int userId)
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

        [HttpPut("{userId}")]
        public async Task<IActionResult> Update([FromBody] RegisterRequest request,int userId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _userService.Update(request, userId);
            return Ok(result);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete([FromRoute] int userId)
        {
            var result = await _userService.Delete(userId);
            if (result.IsSuccessed == false) return Ok(result);
            return Ok(result);
        }

        [HttpGet("getListUser")]
        public async Task<IActionResult> GetListUser()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _userService.GetListUser();
            return Ok(result);
        }

        [HttpGet("getListRole")]
        public async Task<IActionResult> GetListRole()
        {
            var result = await _userService.GetListRole();
            return Ok(result);
        }

    }
}
