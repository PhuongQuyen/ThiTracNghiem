using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ThiTracNghiem_BackEndAPI.Models;
using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.Exams;
namespace ThiTracNghiem_BackEndAPI.Services.ExamServices
{
    public class ExamService : IExamService
    {
        private readonly IConfiguration _configuration;
        private readonly tracnghiemContext _context;
        private readonly IFileProvider _fileProvider;
        public ExamService(IConfiguration configuration, tracnghiemContext context, IFileProvider fileProvider)
        {
            _configuration = configuration;
            _context = context;
            _fileProvider = fileProvider;
        }
        public async Task<ApiResult<string>> Create(ExamViewModel request)
        {
            var exam = new Exams()
            {
                ExamTitle = request.ExamTitle,
                ExamDescription = request.ExamDescription,
                ExamtypeId = request.ExamtypeId,
                TotalQuestions = request.TotalQuestions,
                TimeLimit = request.TimeLimit,
                Status = request.Status
            };
            _context.Exams.Add(exam);
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

        public async Task<ApiResult<bool>> Delete(int examId)
        {

            var exam = await _context.Exams.FindAsync(examId);
            if (exam == null)
            {
                return new ApiResultErrors<bool>("Not found");
            }
            exam.Status = 0;
            var numRowChange = await _context.SaveChangesAsync();
            if (numRowChange > 0)
            {
                return new ApiResultSuccess<bool>();
            }
            return new ApiResultErrors<bool>("Can not detete");
        }

        public async Task<ApiResult<ExamViewModel>> GetById(int examId)
        {
            var query = from e in _context.Exams where e.Id == examId
                        join t in _context.Examtypes on e.ExamtypeId equals t.Id
                        select new { e, t };
            var data = await query
              .Select(x => new ExamViewModel()
            {
                Id = x.e.Id,
                ExamTitle = x.e.ExamTitle,
                ExamDescription = x.e.ExamDescription,
                ExamtypeId = x.e.ExamtypeId,
                TotalQuestions = x.e.TotalQuestions,
                TimeLimit = x.e.TimeLimit,
                Status = x.e.Status,
                ExamTypeTitle = x.t.TypeTitle
            }).FirstOrDefaultAsync();
            return new ApiResultSuccess<ExamViewModel>(data);
        }

        public async Task<ApiResult<ExamInRoomViewModel>> GetByRoomId(int roomId)
        {
            var exam = from r in _context.Rooms where r.Id == roomId
                       join e in _context.Exams on r.ExamId equals e.Id
                       select new { r, e };

            var data = await exam
               .Select(x => new ExamInRoomViewModel()
               {
                   Id = x.e.Id,
                   ExamTitle = x.e.ExamTitle,
                   ExamDescription = x.e.ExamDescription,
                   TotalQuestions = x.e.TotalQuestions,
                   TimeLimit = x.e.TimeLimit,
                   Status = x.e.Status,
                   DateCreated = x.e.DateCreated,
                   IdRoom = x.r.Id,
                   RoomName = x.r.RoomName,
                   RoomCode = x.r.RoomCode,
                   Action = String.Format("<a href = 'javascript:void(0)' data-toggle = 'tooltip' id = 'edit' data-id = '{0}' data-original-title='Edit' class='btn mr-5 btn-xs btn-warning btn-edit'><i class='glyphicon glyphicon-edit'></i> Sửa</a><a href = 'javascript:void(0)' data-toggle='tooltip' id='delete' data-id='{1}' data-original-title='Delete' class='btn btn-xs btn-danger btn-delete'><i class='glyphicon glyphicon-trash'></i> Xóa</a>", x.e.Id, x.e.Id)
               }
               ).FirstOrDefaultAsync();

            return new ApiResultSuccess<ExamInRoomViewModel>(data);
        }

        public async Task<int> GetExamCount()
        {
            return await _context.Exams.CountAsync();
        }

        public async Task<DatatableResult<List<ExamViewModel>>> GetListExam(DatatableRequestBase request)
        {
            var query = from e in _context.Exams
                        join t in _context.Examtypes on e.ExamtypeId equals t.Id
                        select new { e, t };
            int totalRow = await query.CountAsync();
            if (request.sortColumnDirection == "desc")
            {
                switch (request.sortColumn)
                {
                    case "0": query = query.OrderByDescending(e => e.e.Id); break;
                    case "1": query = query.OrderByDescending(e => e.e.ExamTitle); break;
                    case "2": query = query.OrderByDescending(e => e.e.TotalQuestions); break;
                    case "3": query = query.OrderByDescending(e => e.e.TimeLimit); break;
                    case "4": query = query.OrderByDescending(e => e.e.Status); break;
                    case "5": query = query.OrderByDescending(e => e.e.DateCreated); break;
                }

            }
            else if (request.sortColumnDirection == "asc")
            {
                switch (request.sortColumn)
                {
                    case "0": query = query.OrderBy(e => e.e.Id); break;
                    case "1": query = query.OrderBy(e => e.e.ExamTitle); break;
                    case "2": query = query.OrderBy(e => e.e.TotalQuestions); break;
                    case "3": query = query.OrderBy(e => e.e.TimeLimit); break;
                    case "4": query = query.OrderBy(e => e.e.Status); break;
                    case "5": query = query.OrderBy(e => e.e.DateCreated); break;
                }

            }
            var data = await query.Skip(request.Skip)
                .Take(request.PageSize)
                  .Select(x => new ExamViewModel()
                  {
                      Id = x.e.Id,
                      ExamTitle = x.e.ExamTitle,
                      ExamDescription = x.e.ExamDescription,
                      TotalQuestions = x.e.TotalQuestions,
                      TimeLimit = x.e.TimeLimit,
                      Status = x.e.Status,
                      DateCreated = x.e.DateCreated,
                      ExamTypeTitle = x.t.TypeTitle,
                      Action = String.Format("<a href = 'javascript:void(0)' data-toggle = 'tooltip' id = 'edit' data-id = '{0}' data-original-title='Edit' class='btn mr-5 btn-xs btn-warning btn-edit'><i class='glyphicon glyphicon-edit'></i> Sửa</a><a href = 'javascript:void(0)' data-toggle='tooltip' id='delete' data-id='{1}' data-original-title='Delete' class='btn btn-xs btn-danger btn-delete'><i class='glyphicon glyphicon-trash'></i> Xóa</a>", x.e.Id, x.e.Id)
                  }
                  ).ToListAsync();
            var result = new DatatableResult<List<ExamViewModel>>()
            {
                recordsTotal = totalRow,
                recordsFiltered = totalRow,
                Draw = request.Draw,
                Data = data
            };
            return result;

        }

        public async Task<ApiResult<bool>> Update(ExamViewModel request, int examId)
        {
            var exam = await _context.Exams.FindAsync(examId);
            if (exam == null)
            {
                return new ApiResultErrors<bool>("Not found");
            }
            exam.ExamTitle = request.ExamTitle;
            exam.ExamDescription = request.ExamDescription;
            exam.ExamtypeId = request.ExamtypeId;
            exam.TotalQuestions = request.TotalQuestions;
            exam.TimeLimit = request.TimeLimit;
            exam.Status = request.Status;
            await _context.SaveChangesAsync();
            return new ApiResultSuccess<bool>();
        }


    }
}
