using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.Exams;
using ThiTracNghiem_ViewModel.JoinRoom;
using ThiTracNghiem_ViewModel.Rooms;

namespace ThiTracNghiem_BackEndAPI.Services.RoomService
{
    public interface IRoomService
    {
        Task<ApiResult<RoomViewModel>> GetById(int roomId);
        Task<ApiResult<bool>> Delete(int roomId);
        Task<ApiResult<bool>> Update(RoomRequest request, int roomId);
        Task<ApiResult<string>> Create(RoomRequest request);
        Task<DatatableResult<List<RoomViewModel>>> GetListRoom(DatatableRequestBase requestBase);
        Task<ApiResult<bool>> DeleteExamInRoom(int roomId);
        Task<DatatableResult<List<StudentInExamViewModel>>> GetStudentsInExam(DatatableRequestBase requestBase, int roomId);
        Task<List<StudentInExamViewModel>> ExportExcelStudents(int roomId);
        FindRoomViewModel FindRoom(string roomCode);
        JoinRoomViewModel JoinRoom(JoinRoomRequest roomRequest);
        Task<PaginationRequest> GetQuestions(int ExamId,int Page);

        Task<JoinRoomViewModel> SubmitExam(int JoinRoomId,int ExamId,String json);
    }
}
