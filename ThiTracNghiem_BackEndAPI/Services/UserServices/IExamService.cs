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
        Task<DatatableResult<List<ExamViewModel>>> GetListExam(DatatableRequestBase request);
        Task<ApiResult<Exams>> GetById(int examId);
        Task<ApiResult<bool>> Delete(int examId);
        Task<ApiResult<bool>> Update(Exams request, int examId);
        Task<ApiResult<string>> Create(Exams request);

    }
}
