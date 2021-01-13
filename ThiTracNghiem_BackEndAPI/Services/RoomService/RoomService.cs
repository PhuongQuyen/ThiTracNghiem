using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiTracNghiem_BackEndAPI.Models;
using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.Exams;
using ThiTracNghiem_ViewModel.JoinRoom;
using ThiTracNghiem_ViewModel.Rooms;

namespace ThiTracNghiem_BackEndAPI.Services.RoomService
{
    public class RoomService : IRoomService
    {
        private readonly IConfiguration _configuration;
        private readonly tracnghiemContext _context;

        public RoomService(IConfiguration configuration, tracnghiemContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<ApiResult<string>> Create(RoomRequest request)
        {
            Random rnd = new Random();
            request.RoomCode = rnd.Next(1000000, 9999999).ToString();
            var exam = new Exams()
            {
                TotalQuestions = request.TotalQuestions,
                TimeLimit = request.TimeLimit,
                ExamTitle = request.RoomCode
            };

            _context.Exams.Add(exam);
            await _context.SaveChangesAsync();

            var room = new Rooms()
            {
                RoomCode = request.RoomCode,
                ExamId = exam.Id,
                UserId = request.UserId,
                PublicRoom = request.PublicRoom,
                RoomName = request.RoomName,
                Description = request.Description
            };
           
            _context.Rooms.Add(room);
            var numRowChange = await _context.SaveChangesAsync();
            if (numRowChange > 0)
            {
                return new ApiResultSuccess<string>("inserted");
            }
            else
            {
                return new ApiResultErrors<string>("Faild");
            }
        }

        public async Task<ApiResult<bool>> Delete(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return new ApiResultErrors<bool>($"Can not find role with id: {roomId}");
            _context.Rooms.Remove(room);
            var numRowChange = await _context.SaveChangesAsync();
            if (numRowChange > 0)
            {
                return new ApiResultSuccess<bool>();
            }
            return new ApiResultErrors<bool>("Can not detete");
        }

        public async Task<ApiResult<bool>> DeleteExamInRoom(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return new ApiResultErrors<bool>($"Can not find role with id: {roomId}");
            room.ExamId = 0;
            var numRowChange = await _context.SaveChangesAsync();
            if (numRowChange > 0)
            {
                return new ApiResultSuccess<bool>();
            }
            return new ApiResultErrors<bool>("Can not detete");
        }

        public async Task<ApiResult<RoomViewModel>> GetById(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            var exam = await _context.Exams.FindAsync(room.ExamId);
            var currentQuestion = await _context.Questiondetails.Where(q => q.ExamId == exam.Id).ToListAsync();
            var userCount = await _context.Joinroom.Where(q => q.RoomId == room.Id).ToListAsync();
            if (room == null)
            {
                return new ApiResultErrors<RoomViewModel>("not found");
            }
            var roomViewModel = new RoomViewModel()
            {
                RoomName = room.RoomName,
                TotalQuestions = exam.TotalQuestions,
                TimeLimit = exam.TimeLimit,
                PublicRoom = room.PublicRoom,
                ExamId = room.ExamId,
                RoomCode = room.RoomCode
            };
            return new ApiResultSuccess<RoomViewModel>(roomViewModel);
        }

        public async Task<DatatableResult<List<RoomViewModel>>> GetListRoom(DatatableRequestBase request)
        {
            var query = from r in _context.Rooms
                        join e in _context.Exams on r.ExamId equals e.Id
                        select new { r, e };

            if (!String.IsNullOrEmpty(request.searchValue))
            {
                query = query.Where(x => x.r.RoomName.Contains(request.searchValue));
            }

            int totalRow = await query.CountAsync();

            if (request.sortColumnDirection == "desc")
            {
                switch (request.sortColumn)
                {
                    case "0": query = query.OrderByDescending(r => r.r.Id); break;
                    case "1": query = query.OrderByDescending(r => r.r.RoomCode); break;
                    case "2": query = query.OrderByDescending(r => r.r.RoomName); break;
                    case "3": query = query.OrderByDescending(r => r.e.TotalQuestions); break;
                    case "5": query = query.OrderByDescending(r => r.e.TimeLimit); break;
                    case "7": query = query.OrderByDescending(r => r.r.PublicRoom); break;
                }

            }
            else if (request.sortColumnDirection == "asc")
            {
                switch (request.sortColumn)
                {
                    case "0": query = query.OrderBy(r => r.r.Id); break;
                    case "1": query = query.OrderBy(r => r.r.RoomCode); break;
                    case "2": query = query.OrderBy(r => r.r.RoomName); break;
                    case "3": query = query.OrderBy(r => r.e.TotalQuestions); break;
                    case "5": query = query.OrderBy(r => r.e.TimeLimit); break;
                    case "7": query = query.OrderBy(r => r.r.PublicRoom); break;
                }

            }

            var data = await query.Skip(request.Skip).Take(request.PageSize)
                  .Select(x => new RoomViewModel()
                  {
                      Id = x.r.Id,
                      RoomCode = x.r.RoomCode,
                      RoomName = String.Format("<a href='/creator/examinroom/{0}'>{1}</ a>",x.r.Id, x.r.RoomName),
                      TotalQuestions = x.e.TotalQuestions,
                      TimeLimit = x.e.TimeLimit,
                      PublicRoom = x.r.PublicRoom,
                      CurrentQuestions = 20,//2 cái nè chưa truy vấn ra đc em xem viết đc thì viết
                      UserCounts = 20,
                      Action = String.Format("<a href = 'javascript:void(0)' data-toggle = 'tooltip' id = 'edit' data-id = '{0}' data-original-title='Edit' class='btn mr-5 btn-xs btn-warning btn-edit'><i class='glyphicon glyphicon-edit'></i> Sửa</a><a href = 'javascript:void(0)' data-toggle='tooltip' id='delete' data-id='{1}' data-original-title='Delete' class='btn btn-xs btn-danger btn-delete'><i class='glyphicon glyphicon-trash'></i> Xóa</a>", x.r.Id, x.r.Id)
                  }
                  ).ToListAsync();

            var result = new DatatableResult<List<RoomViewModel>>()
            {
                recordsTotal = totalRow,
                recordsFiltered = totalRow,
                Draw = request.Draw,
                Data = data
            };
            return result;
        }
        int getCurrentQuestion(int id)
        {
            return _context.Questiondetails.Where(q => q.ExamId == id).ToList().Count();
        }
        int getUserQuestion(int id)
        {
            return _context.Joinroom.Where(q => q.RoomId == id).ToList().Count();
        }
        public async Task<ApiResult<bool>> Update(RoomRequest request, int roomId)
        {
            var rooom = await _context.Rooms.FindAsync(roomId);
            var exam = await _context.Exams.FindAsync(rooom.ExamId);
            if (rooom == null)
            {
                return new ApiResultErrors<bool>("Not found");
            }

            exam.TotalQuestions = request.TotalQuestions;
            exam.TimeLimit = request.TimeLimit;

            rooom.PublicRoom = request.PublicRoom;
            rooom.RoomName = request.RoomName;
            rooom.Description = request.Description;

            await _context.SaveChangesAsync();
            return new ApiResultSuccess<bool>();
        }

        public async Task<DatatableResult<List<StudentInExamViewModel>>> GetStudentsInExam(DatatableRequestBase request, int roomId)
        {
            var query = from e in _context.Joinroom
                        where e.RoomId == roomId 
                        select e;

            if (!String.IsNullOrEmpty(request.searchValue))
            {
                query = query.Where(x => x.FullName.Contains(request.searchValue));
            }

            int totalRow = await query.CountAsync();

            if (request.sortColumnDirection == "desc")
            {
                switch (request.sortColumn)
                {
                    case "0": query = query.OrderByDescending(r => r.FullName); break;
                    case "1": query = query.OrderByDescending(r => r.Mssv); break;
                    case "2": query = query.OrderByDescending(r => r.Score); break;
                    case "3": query = query.OrderByDescending(r => r.TimeSubmitExam); break;
                }

            }
            else if (request.sortColumnDirection == "asc")
            {
                switch (request.sortColumn)
                {
                    case "0": query = query.OrderByDescending(r => r.FullName); break;
                    case "1": query = query.OrderByDescending(r => r.Mssv); break;
                    case "2": query = query.OrderByDescending(r => r.Score); break;
                    case "3": query = query.OrderByDescending(r => r.TimeSubmitExam); break;
                }

            }

            var data = await query.Skip(request.Skip).Take(request.PageSize)
                  .Select(x => new StudentInExamViewModel()
                  {
                      HoTen = x.FullName,
                      Mssv = x.Mssv,
                      Score =  x.Score,
                      Timesubmit = x.TimeSubmitExam
                  }
                  ).ToListAsync();

            var result = new DatatableResult<List<StudentInExamViewModel>>()
            {
                recordsTotal = totalRow,
                recordsFiltered = totalRow,
                Draw = request.Draw,
                Data = data
            };
            return result;
        }

        public Task<List<StudentInExamViewModel>> ExportExcelStudents(int roomId)
        {
            var query = from e in _context.Joinroom
                        where e.RoomId == roomId
                        orderby e.Score ascending
                        select e;
            var data = query.Select(x => new StudentInExamViewModel()
                 {
                     HoTen = x.FullName,
                     Mssv = x.Mssv,
                     Score = x.Score,
                     Timesubmit = x.TimeSubmitExam
                 }
                 ).ToListAsync();
            return data;
        }
    
        public FindRoomViewModel FindRoom(string roomCode)
        {
            var query = from r in _context.Rooms
                        join ex in _context.Exams on r.ExamId equals ex.Id
                        where r.RoomCode == roomCode
                        select new { r, ex };
            var data = query.Select(x => new FindRoomViewModel()
            {
                Id = x.r.Id,
              Description =x.r.Description,
              ExamId = x.r.ExamId,
              RoomCode = x.r.RoomCode,
               RoomName = x.r.RoomName,
               exam = new ExamViewModel()
               {
                   Id = x.ex.Id,
                   ExamTitle = x.ex.ExamTitle,
                   ExamDescription = x.ex.ExamDescription,
                   TimeLimit= x.ex.TimeLimit,
                   TotalQuestions = x.ex.TotalQuestions,
                   DateCreated = x.ex.DateCreated
               }
            }).First();
            return data;
        }

        public JoinRoomViewModel JoinRoom(JoinRoomRequest roomRequest)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationRequest> GetQuestions(int examId)
        {
            throw new NotImplementedException();
        }
    }
}
