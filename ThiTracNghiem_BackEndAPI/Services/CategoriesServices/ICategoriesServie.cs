using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiTracNghiem_ViewModel.Blog;
using ThiTracNghiem_ViewModel.Commons;

namespace ThiTracNghiem_BackEndAPI.Services.CategoriesServices
{
    public interface ICategoriesServie
    {
        Task<DatatableResult<List<CategoriesViewModel>>> GetListCategories(DatatableRequestBase request);
        Task<ApiResult<CategoriesViewModel>> GetById(int categoriesId);
        Task<ApiResult<bool>> Delete(int categoriesId);
        Task<ApiResult<bool>> Update(CategoriesRequest request, int categoriesId);
        Task<ApiResult<string>> Create(CategoriesRequest request);
    }
}
