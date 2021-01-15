using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
            try
            {
                var data = query.Select(x => new FindRoomViewModel()
                {
                    Id = x.r.Id,
                    Description = x.r.Description,
                    ExamId = x.r.ExamId,
                    PublicRoom = x.r.PublicRoom,
                    RoomCode = x.r.RoomCode,
                    RoomName = x.r.RoomName,
                    exam = new ExamViewModel()
                    {
                        Id = x.ex.Id,
                        ExamTitle = x.ex.ExamTitle,
                        ExamDescription = x.ex.ExamDescription,
                        TimeLimit = x.ex.TimeLimit,
                        TotalQuestions = x.ex.TotalQuestions,
                        DateCreated = x.ex.DateCreated
                    }
                }).First();
            return data;
            }catch(InvalidOperationException e)
            {
                return null;
            }
        }

        public JoinRoomViewModel JoinRoom(JoinRoomRequest roomRequest)
        {
            if (roomRequest.RoomId ==0) return null;
            var query = from r in _context.Rooms
                        where r.Id == roomRequest.RoomId
                        select r;
            try
            {
                var room = query.First();
                if (roomRequest.UserId !=0)
                {
                    var userqr = from u in _context.Users where u.Id == roomRequest.UserId select u;
                    var user = userqr.First();
                    var joinRoom = new Joinroom()
                    {
                        RoomId = roomRequest.RoomId,
                        UserId = roomRequest.UserId,
                       
                    };
                    if (user != null)
                    {
                        joinRoom.Email = user.Email;
                        joinRoom.FullName = user.FirstName + user.LastName;
                    }
                    _context.Joinroom.Add(joinRoom);
                    _context.SaveChanges();
                    return new JoinRoomViewModel()
                    {
                        Id = joinRoom.Id,
                        RoomId = joinRoom.RoomId,
                        UserId = joinRoom.UserId
                    };
                }
                else
                {
                    if(roomRequest.FullName.Equals("") || roomRequest.Email.Equals("") || roomRequest.Mssv.Equals(""))
                    {
                        return null;
                    }
                    else
                    {
                        var joinRoom = new Joinroom()
                        {
                            RoomId = roomRequest.RoomId,
                            Mssv = roomRequest.Mssv,
                            Email= roomRequest.Email,
                            FullName= roomRequest.FullName
                        };
                        _context.Joinroom.Add(joinRoom);
                        _context.SaveChanges();
                        return new JoinRoomViewModel()
                        {
                            Id = joinRoom.Id,
                            RoomId = roomRequest.RoomId,
                            Mssv = roomRequest.Mssv,
                            Email = roomRequest.Email,
                            FullName = roomRequest.FullName
                        };
                    }
                }
            }catch(InvalidOperationException e)
            {
                return null;
            }
        }

        public async Task<PaginationRequest> GetQuestions(int ExamId,int Page)
        {
            var query = from q in _context.Questions
                        join qd in _context.Questiondetails on q.Id equals qd.QuestionId
                        join a in _context.Answers on q.Id equals a.QuestionId
                        join e in _context.Exams on qd.ExamId equals e.Id
                        where qd.ExamId == ExamId
                        orderby q.Id
                        select new { q, a };
            int total = await query.CountAsync();
            PaginationRequest paginationRequest = new PaginationRequest(total, Page);
            var data = await query.Skip(paginationRequest.From-1).Take(10).Select(x => new QuestionAndAnswer() {
                Id = x.q.Id,
                QuestionContent = x.q.QuestionContent,
                QuestionType = x.q.QuestionType,
                A = x.a.A,
                B= x.a.B,
                C = x.a.C,
                D=x.a.D
            }).ToListAsync() ;
            paginationRequest.Data = data;
            return paginationRequest;
        }

        public async Task<ScoreExamViewModel> SubmitExam(int JoinRoomId, int ExamId, string json)
        {
            var query = from jr in _context.Joinroom where jr.Id == JoinRoomId select jr;
            var joinRoom = query.First();
            if (joinRoom == null) return null;
            var queryqs = from q in _context.Questions
                    join qd in _context.Questiondetails on q.Id equals qd.QuestionId
                    join a in _context.Answers on q.Id equals a.QuestionId
                    join e in _context.Exams on qd.ExamId equals e.Id
                    where qd.ExamId == ExamId
                    orderby q.Id
                    select new { q, a };
            var questions = await queryqs.Select(x=> new QuestionAndAnswer(){
            Id = x.q.Id,
            QuestionType = x.q.QuestionType,
            CorrectAnswers = x.a.CorrectAnswers
            }).ToListAsync();

            var data = JsonSerializer.Deserialize<Dictionary<string,string>>(json);
            int i = 0;
            int score = 0;
            bool flag = true;
            foreach (var kv in data)
            {
                if (questions[i].Id == int.Parse(kv.Key))
                {

                    if (questions[i].CorrectAnswers.Length == 1)
                    {
                        if (questions[i].CorrectAnswers.Equals(kv.Value))
                        {
                            score++;
                        }
                        i++;
                        continue;
                    }
                    else
                    {
                        var arrquestion = questions[i].CorrectAnswers.Split(",");
                        var arrchose = kv.Value.Split(",");
                        i++;
                        if (arrquestion.Length != arrchose.Length) continue;
                        flag = true;
                        foreach (var elem in arrchose)
                        {
                            if (!arrquestion.Contains(elem))
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag) score++;
                    }


                }
            }
            joinRoom.Score = score;
            joinRoom.TimeSubmitExam = DateTime.Now;
            _context.SaveChanges();
            JoinRoomViewModel joinRoomViewModel = new JoinRoomViewModel()
            {
                Id = joinRoom.Id,
                RoomId = joinRoom.RoomId,
                Score = score,
                Email = joinRoom.Email,
                FullName = joinRoom.FullName,
                Mssv = joinRoom.Mssv,
                UserId = joinRoom.UserId,
                TimeSubmitExam = DateTime.Now
            };
            ScoreExamViewModel s = new ScoreExamViewModel()
            {
                Answers = questions,
                Joinroom = joinRoomViewModel,
                Score = score,
            };
            return s;
        }
    }
}
