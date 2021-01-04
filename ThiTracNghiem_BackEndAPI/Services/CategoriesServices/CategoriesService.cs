using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiTracNghiem_BackEndAPI.Models;
using ThiTracNghiem_ViewModel.Blog;
using ThiTracNghiem_ViewModel.Commons;

namespace ThiTracNghiem_BackEndAPI.Services.CategoriesServices
{
    public class CategoriesService : ICategoriesServie
    {
        private readonly IConfiguration _configuration;
        private readonly tracnghiemContext _context;

        public CategoriesService(IConfiguration configuration, tracnghiemContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<ApiResult<string>> Create(CategoriesRequest request)
        {
            var postcategories = new Postcategories()
            {
                CategoryName = request.CategoryName,
                CategoryDescription = request.CategoryDescription
            };

            _context.Postcategories.Add(postcategories);
            var numRowChange = await _context.SaveChangesAsync();
            if (numRowChange > 0)
            {
                return new ApiResultSuccess<string>("inserted");
            }
            else
            {
                return new ApiResultErrors<string>("Faild");
            }
        }

        public async Task<ApiResult<bool>> Delete(int categoriesId)
        {
            var postcategories = await _context.Postcategories.FindAsync(categoriesId);
            if (postcategories == null) return new ApiResultErrors<bool>($"Can not find question with id: {categoriesId}");
            _context.Postcategories.Remove(postcategories);
            var numRowChange = await _context.SaveChangesAsync();
            if (numRowChange > 0)
            {
                return new ApiResultSuccess<bool>();
            }
            return new ApiResultErrors<bool>("Can not detete");
        }

        public async Task<ApiResult<CategoriesViewModel>> GetById(int categoriesId)
        {
            var postcategories = await _context.Postcategories.FindAsync(categoriesId);
            if (postcategories == null)
            {
                return new ApiResultErrors<CategoriesViewModel>("not found");
            }
            var categoriesViewModel = new CategoriesViewModel()
            {
                ID = postcategories.Id,
                CategoryName = postcategories.CategoryName,
                CategoryDescription = postcategories.CategoryDescription
            };
            return new ApiResultSuccess<CategoriesViewModel>(categoriesViewModel);
        }

        public async Task<DatatableResult<List<CategoriesViewModel>>> GetListCategories(DatatableRequestBase request)
        {
            var query = from p in _context.Postcategories
                        select p;

            if (!String.IsNullOrEmpty(request.searchValue))
            {
                query = query.Where(x => x.CategoryDescription.Contains(request.searchValue) || x.CategoryDescription.Contains(request.searchValue));
            }

            int totalRow = await query.CountAsync();

            if (request.sortColumnDirection == "desc")
            {
                switch (request.sortColumn)
                {
                    case "0": query = query.OrderByDescending(r => r.Id); break;
                    case "1": query = query.OrderByDescending(r => r.CategoryName); break;
                    case "2": query = query.OrderByDescending(r => r.CategoryDescription); break;
                }

            }
            else if (request.sortColumnDirection == "asc")
            {
                switch (request.sortColumn)
                {
                    case "0": query = query.OrderBy(r => r.Id); break;
                    case "1": query = query.OrderBy(r => r.CategoryName); break;
                    case "2": query = query.OrderBy(r => r.CategoryDescription); break;
                }

            }

            var data = await query.Skip(request.Skip).Take(request.PageSize)
                  .Select(x => new CategoriesViewModel()
                  {
                      ID = x.Id,
                      CategoryName = x.CategoryName,
                      CategoryDescription = x.CategoryDescription,
                      Action = String.Format("<a href = 'javascript:void(0)' data-toggle = 'tooltip' id = 'edit' data-id = '{0}' data-original-title='Edit' class='btn mr-5 btn-xs btn-warning btn-edit'><i class='glyphicon glyphicon-edit'></i> Sửa</a><a href = 'javascript:void(0)' data-toggle='tooltip' id='delete' data-id='{1}' data-original-title='Delete' class='btn btn-xs btn-danger btn-delete'><i class='glyphicon glyphicon-trash'></i> Xóa</a>", x.Id, x.Id)
                  }).ToListAsync();

            var result = new DatatableResult<List<CategoriesViewModel>>()
            {
                recordsTotal = totalRow,
                recordsFiltered = totalRow,
                Draw = request.Draw,
                Data = data
            };
            return result;
        }

        public async Task<ApiResult<bool>> Update(CategoriesRequest request, int categoriesId)
        {
            var postcategories = await _context.Postcategories.FindAsync(categoriesId);
            if (postcategories == null)
            {
                return new ApiResultErrors<bool>("Not found");
            }
            postcategories.CategoryName = request.CategoryName;
            postcategories.CategoryDescription = request.CategoryDescription;

            await _context.SaveChangesAsync();
            return new ApiResultSuccess<bool>();
        }
    }
}
