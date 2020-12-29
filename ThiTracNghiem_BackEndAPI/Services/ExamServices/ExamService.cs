using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
        public ExamService(IConfiguration configuration, tracnghiemContext context)
        {
            _configuration = configuration;
            _context = context;
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

            var user = await _context.Exams.FindAsync(examId);
            if (user != null)
            {
                return new ApiResultErrors<bool>("Not found");
            }
            _context.Exams.Remove(user);
            var numRowChange = await _context.SaveChangesAsync();
            if (numRowChange > 0)
            {
                return new ApiResultSuccess<bool>();
            }
            return new ApiResultErrors<bool>("Can not detete");
        }

        public async Task<ApiResult<ExamViewModel>> GetById(int examId)
        {
            var exam = await _context.Exams.FindAsync(examId);
            var examViewModel = new ExamViewModel()
            {
                Id = exam.Id,
                ExamTitle = exam.ExamTitle,
                ExamDescription = exam.ExamDescription,
                ExamtypeId = exam.ExamtypeId,
                TotalQuestions = exam.TotalQuestions,
                TimeLimit = exam.TimeLimit,
                Status = exam.Status
            };
            return new ApiResultSuccess<ExamViewModel>(examViewModel);
        }

        public async Task<DatatableResult<List<ExamViewModel>>> GetListExam(DatatableRequestBase request)
        {
            var query = from e in _context.Exams select e;
            int totalRow = await query.CountAsync();
            if (request.sortColumnDirection == "desc")
            {
                switch (request.sortColumn)
                {
                    case "0": query = query.OrderByDescending(e => e.Id); break;
                    case "1": query = query.OrderByDescending(e => e.ExamTitle); break;
                    case "2": query = query.OrderByDescending(e => e.TotalQuestions); break;
                    case "3": query = query.OrderByDescending(e => e.TimeLimit); break;
                    case "4": query = query.OrderByDescending(e => e.Status); break;
                    case "5": query = query.OrderByDescending(e => e.DateCreated); break;
                }

            }
            else if (request.sortColumnDirection == "asc")
            {
                switch (request.sortColumn)
                {
                    case "0": query = query.OrderBy(e => e.Id); break;
                    case "1": query = query.OrderBy(e => e.ExamTitle); break;
                    case "2": query = query.OrderBy(e => e.TotalQuestions); break;
                    case "3": query = query.OrderBy(e => e.TimeLimit); break;
                    case "4": query = query.OrderBy(e => e.Status); break;
                    case "5": query = query.OrderBy(e => e.DateCreated); break;
                }

            }
            var data = await query.Skip(request.Skip)
                .Take(request.PageSize)
                  .Select(x => new ExamViewModel()
                  {
                      Id = x.Id,
                      ExamTitle = x.ExamTitle,
                      ExamDescription = x.ExamDescription,
                      TotalQuestions = x.TotalQuestions,
                      TimeLimit = x.TimeLimit,
                      Status = x.Status,
                      DateCreated = x.DateCreated,
                      Action = String.Format("<a href = 'javascript:void(0)' data-toggle = 'tooltip' id = 'edit' data-id = '{0}' data-original-title='Edit' class='btn mr-5 btn-xs btn-warning btn-edit'><i class='glyphicon glyphicon-edit'></i> Sửa</a><a href = 'javascript:void(0)' data-toggle='tooltip' id='delete' data-id='{1}' data-original-title='Delete' class='btn btn-xs btn-danger btn-delete'><i class='glyphicon glyphicon-trash'></i> Xóa</a>", x.Id, x.Id)
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
            if (exam != null)
            {
                return new ApiResultErrors<bool>("Not found");
            }
            exam.ExamTitle = request.ExamTitle;
            exam.ExamDescription = request.ExamDescription;
            exam.ExamtypeId = request.ExamtypeId;
            exam.TotalQuestions = request.TotalQuestions;
            exam.TimeLimit = request.TimeLimit;
            exam.Status = request.Status;
            var numRowChange = await _context.SaveChangesAsync();
            if (numRowChange > 0)
            {
                return new ApiResultSuccess<bool>();
            }
            else
            {
                return new ApiResultErrors<bool>("Faild");
            }
        }
    }
}
