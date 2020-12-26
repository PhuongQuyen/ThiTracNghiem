using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.Exams;
using ThiTracNghiem_BackEndAPI.Models;
namespace ThiTracNghiem_BackEndAPI.Services.UserServices
{
    public interface IExamService
    {
        Task<ApiResult<List<Exams>>> GetListExam();
        Task<ApiResult<Exams>> GetById(int examId);
        Task<ApiResult<bool>> Delete(int examId);
        Task<ApiResult<bool>> Update(ExamViewModel request, int examId);
        Task<ApiResult<string>> Create(ExamViewModel request);

    }
}
