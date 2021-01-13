using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using ThiTracNghiem_BackEndAPI.Models;
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
        [HttpGet("DeleteExamInRoom/{roomId}")]
        public async Task<IActionResult> DeleteExamInRoom([FromRoute] int roomId)
        {
            var result = await _roomService.DeleteExamInRoom(roomId);
            if (result.IsSuccessed == false) return Ok(result);
            return Ok(result);
        }

        [HttpGet("GetStudentsInRoom/{roomId}")]
        public async Task<IActionResult> GetStudentsInRoom([FromRoute] int roomId)
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
            var result = await _roomService.GetStudentsInExam(requestBase, roomId);
            return Ok(result);
        }
        [HttpGet("ExportStudent/{roomId}")]
        [AllowAnonymous]
        public async Task<IActionResult> ExportStudent(int roomId)
        {
            var data = await _roomService.ExportExcelStudents(roomId);
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet 1");
                workSheet.Cells[1, 1].Value = "Họ Tên";
                workSheet.Cells[1, 2].Value = "MSSV";
                workSheet.Cells[1, 3].Value = "Điểm";
                workSheet.Cells[1, 4].Value = "Thời gian nộp bài";
                int recordIndex = 2;
                foreach (var student in data)
                {
                    workSheet.Cells[recordIndex, 1].Value = student.HoTen;
                    workSheet.Cells[recordIndex, 2].Value = student.Mssv;
                    workSheet.Cells[recordIndex, 3].Value = student.Score;
                    workSheet.Cells[recordIndex, 4].Value = student.Timesubmit.ToString();
                    recordIndex++;
                }
                workSheet.Column(1).AutoFit();
                workSheet.Column(2).AutoFit();
                workSheet.Column(3).AutoFit();
                workSheet.Column(4).AutoFit();
                excel.Workbook.Properties.Title = "Student";
                var fileName = "Student.xlsx";
                var mimeType = "application/vnd.ms-excel";
                return File(excel.GetAsByteArray(), mimeType, fileName);
            }
        }

        [HttpGet("examreal/findroom")]
        [AllowAnonymous]
        public IActionResult FindRoom()
        {
            var RoomCode = HttpContext.Request.Query["roomcode"];
            var room = _roomService.FindRoom(RoomCode);
            if(room != null)
            {
                return new JsonResult(room);
            }
            else
            {
                Response.StatusCode = 401;
                return Ok(new ApiResultErrors<string>() { Message= "Room not found" });
            }

        }
    }
}
