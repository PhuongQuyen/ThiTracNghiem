using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.Exams;
using ThiTracNghiem_BackEndAPI.Models;
using ThiTracNghiem_ViewModel.Rooms;
using Microsoft.AspNetCore.Mvc;

namespace ThiTracNghiem_BackEndAPI.Services.ExamServices
{
    public interface IExamService
    {
        Task<DatatableResult<List<ExamViewModel>>> GetListExam(DatatableRequestBase request);
        Task<ApiResult<ExamViewModel>> GetById(int examId);
        Task<ApiResult<bool>> Delete(int examId);
        Task<ApiResult<bool>> Update(ExamViewModel request, int examId);
        Task<ApiResult<string>> Create(ExamViewModel request);
        Task<int> GetExamCount();
        Task<ApiResult<ExamInRoomViewModel>> GetByRoomId(int roomId); //creator
    
    }
}
