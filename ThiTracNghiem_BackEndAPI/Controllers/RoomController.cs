using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThiTracNghiem_BackEndAPI.Services.RoomService;
using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.Rooms;

namespace ThiTracNghiem_BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("GetById/{roomId}")]
        public async Task<IActionResult> GetById(int roomId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _roomService.GetById(roomId);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] RoomRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _roomService.Create(request);
            return Ok(result);
        }

        [HttpPost("update/{roomId}")]
        public async Task<IActionResult> Update([FromForm] RoomRequest request, int roomId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _roomService.Update(request, roomId);
            return Ok(result);
        }

        [HttpGet("delete/{roomId}")]
        public async Task<IActionResult> Delete([FromRoute] int roomId)
        {
            var result = await _roomService.Delete(roomId);
            if (result.IsSuccessed == false) return Ok(result);
            return Ok(result);
        }

        [HttpGet("GetListRoom")]
        public async Task<IActionResult> GetListRoom()
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
            var result = await _roomService.GetListRoom(requestBase);
            return Ok(result);
        }
     
    }
}
