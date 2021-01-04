using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.Exams;

namespace ThiTracNghiem_BackEndAPI.Services.ModuleService
{
    public interface IModuleService
    {
        Task<DatatableResult<List<ModuleViewModel>>> GetListModule(DatatableRequestBase request);
        Task<ApiResult<ModuleViewModel>> GetById(int moduleId);
        Task<ApiResult<bool>> Update(ModuleRequest request, int moduleId);
        Task<ApiResult<string>> Create(ModuleRequest request);
        Task<ApiResult<bool>> Delete(int moduleId);
    }
}
