using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.Users;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ThiTracNghiem_ViewModel.Roles;

namespace ThiTracNghiem_WebAdmin.Services.Users
{
    public interface IUserAPIClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);
        Task<ApiResult<UserViewModel>> getUserById(Guid userId);
        Task<ApiResult<List<UserViewModel>>> getListUser();
        Task<ApiResult<List<RoleViewModel>>> getListRole();
        Task<ApiResult<string>> Register(RegisterRequest request);
        Task<ApiResult<string>> Delete(Guid userId);
        Task<ApiResult<bool>> Update(RegisterRequest request, int userId);
        Task<ApiResult<UserViewModel>> GetUserByEmail(string email);
        Task<ApiResult<UserViewModel>> GetUserByUserName(string userName);
    }
}
