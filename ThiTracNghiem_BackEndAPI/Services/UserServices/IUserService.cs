using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.Roles;
using ThiTracNghiem_ViewModel.Users;

namespace ThiTracNghiem_BackEndAPI.Services.UserServices
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authencate(LoginRequest request);
        Task<ApiResult<string>> GetPasswordResetToken(string email);
        Task<ApiResult<string>> Register(RegisterRequest request);
        Task<ApiResult<UserViewModel>> GetById(int userId);
        Task<ApiResult<List<UserViewModel>>> GetListUser();
        Task<ApiResult<UserViewModel>> GetByEmail(string email);
        Task<ApiResult<UserViewModel>> GetByUserName(string userName);
        Task<ApiResult<bool>> Delete(int userId);
        Task<ApiResult<bool>> Update(RegisterRequest request, int userId);
        Task<ApiResult<List<RoleViewModel>>> GetListRole();
    }
}
