using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiTracNghiem_BackEndAPI.Models;
using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.Exams;

namespace ThiTracNghiem_BackEndAPI.Services.ModuleService
{
    public class ModuleService : IModuleService
    {
        private readonly IConfiguration _configuration;
        private readonly tracnghiemContext _context;

        public ModuleService(IConfiguration configuration, tracnghiemContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<ApiResult<string>> Create(ModuleRequest request)
        {
            var module = new Modules()
            {
                ModuleName = request.ModuleName,
                ModuleDescription = request.ModuleDescription
            };

            _context.Modules.Add(module);
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

        public async Task<ApiResult<bool>> Delete(int moduleId)
        {
            var module = await _context.Modules.FindAsync(moduleId);
            if (module == null) return new ApiResultErrors<bool>($"Can not find question with id: {moduleId}");
            _context.Modules.Remove(module);
            var numRowChange = await _context.SaveChangesAsync();
            if (numRowChange > 0)
            {
                return new ApiResultSuccess<bool>();
            }
            return new ApiResultErrors<bool>("Can not detete");
        }

        public async Task<ApiResult<ModuleViewModel>> GetById(int moduleId)
        {
            var module = await _context.Modules.FindAsync(moduleId);
            if (module == null)
            {
                return new ApiResultErrors<ModuleViewModel>("not found");
            }
            var moduleViewModel = new ModuleViewModel()
            {
                ID = module.Id,
                ModuleName = module.ModuleName,
                ModuleDescription = module.ModuleDescription
            };
            return new ApiResultSuccess<ModuleViewModel>(moduleViewModel);
        }

        public async Task<DatatableResult<List<ModuleViewModel>>> GetListModule(DatatableRequestBase request)
        {
            var query = from m in _context.Modules
                        select m;

            if (!String.IsNullOrEmpty(request.searchValue))
            {
                query = query.Where(x => x.ModuleName.Contains(request.searchValue) || x.ModuleDescription.Contains(request.searchValue));
            }

            int totalRow = await query.CountAsync();

            if (request.sortColumnDirection == "desc")
            {
                switch (request.sortColumn)
                {
                    case "0": query = query.OrderByDescending(r => r.Id); break;
                    case "1": query = query.OrderByDescending(r => r.ModuleName); break;
                    case "2": query = query.OrderByDescending(r => r.ModuleDescription); break;
                }

            }
            else if (request.sortColumnDirection == "asc")
            {
                switch (request.sortColumn)
                {
                    case "0": query = query.OrderBy(r => r.Id); break;
                    case "1": query = query.OrderBy(r => r.ModuleName); break;
                    case "2": query = query.OrderBy(r => r.ModuleDescription); break;
                }

            }

            var data = await query.Skip(request.Skip).Take(request.PageSize)
                  .Select(x => new ModuleViewModel()
                  {
                      ID = x.Id,
                      ModuleName = x.ModuleName,
                      ModuleDescription = x.ModuleDescription,
                      Action = String.Format("<a href = 'javascript:void(0)' data-toggle = 'tooltip' id = 'edit' data-id = '{0}' data-original-title='Edit' class='btn mr-5 btn-xs btn-warning btn-edit'><i class='glyphicon glyphicon-edit'></i> Sửa</a><a href = 'javascript:void(0)' data-toggle='tooltip' id='delete' data-id='{1}' data-original-title='Delete' class='btn btn-xs btn-danger btn-delete'><i class='glyphicon glyphicon-trash'></i> Xóa</a>", x.Id, x.Id)
                  }).ToListAsync();

            var result = new DatatableResult<List<ModuleViewModel>>()
            {
                recordsTotal = totalRow,
                recordsFiltered = totalRow,
                Draw = request.Draw,
                Data = data
            };
            return result;
        }

        public async Task<ApiResult<bool>> Update(ModuleRequest request, int moduleId)
        {
            var module = await _context.Modules.FindAsync(moduleId);
            if (module == null)
            {
                return new ApiResultErrors<bool>("Not found");
            }
            module.ModuleName = request.ModuleName;
            module.ModuleDescription = request.ModuleDescription;

            await _context.SaveChangesAsync();
            return new ApiResultSuccess<bool>();
        }
    }
}
