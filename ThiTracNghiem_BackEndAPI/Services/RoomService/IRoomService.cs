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
        Task<ApiResult<RoomRequest>> GetById(int roleId);
        Task<ApiResult<bool>> Delete(int roleId);
        Task<ApiResult<bool>> Update(RoomRequest request, int roleId);
        Task<ApiResult<string>> Create(RoomRequest request);
        Task<DatatableResult<List<RoomViewModel>>> GetListRoom(DatatableRequestBase requestBase);
    }
}
