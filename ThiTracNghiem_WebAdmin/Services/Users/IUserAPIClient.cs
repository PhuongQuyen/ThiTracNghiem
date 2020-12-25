using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.Users;
using System;
using System.Threading.Tasks;

namespace ThiTracNghiem_WebAdmin.Services.Users
{
    public interface IUserAPIClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);
        Task<ApiResult<UserViewModel>> getUserById(Guid userId);
        Task<ApiResult<string>> Register(RegisterRequest request);
        Task<ApiResult<string>> Delete(Guid userId);
        //Task<ApiResult<string>> Update(Guid userId,UserUpdateRequest request);
        Task<ApiResult<UserViewModel>> GetUserByEmail(string email);
        Task<ApiResult<UserViewModel>> GetUserByUserName(string userName);
    }
}
