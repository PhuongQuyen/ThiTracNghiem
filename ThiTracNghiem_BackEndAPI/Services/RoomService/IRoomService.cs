using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiTracNghiem_ViewModel.Commons;
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

    }
}
