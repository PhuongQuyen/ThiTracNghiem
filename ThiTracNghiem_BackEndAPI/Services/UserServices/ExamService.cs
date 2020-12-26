using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiTracNghiem_BackEndAPI.Models;
using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.Exams;

namespace ThiTracNghiem_BackEndAPI.Services.UserServices
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

        public async Task<ApiResult<Exams>> GetById(int examId)
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
            return new ApiResultSuccess<Exams>(exam);
        }

        public async Task<ApiResult<List<Exams>>> GetListExam()
        {
            var exams =  _context.Exams.ToList();
            return new ApiResultSuccess<List<Exams>>(exams);
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
