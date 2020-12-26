using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.Roles;
using ThiTracNghiem_ViewModel.Users;

namespace ThiTracNghiem_BackEndAPI.Services.RoleServices
{
    public interface IRoleService
    {
        Task<ApiResult<RoleViewModel>> GetById(int roleId);
        Task<ApiResult<bool>> Delete(int roleId);
        Task<ApiResult<bool>> Update(RoleViewModel request, int roleId);
        Task<ApiResult<string>> Create(RoleViewModel request);
        Task<DatatableResult<List<RoleViewModel>>> GetListRole(DatatableRequestBase requestBase);
    }
}
