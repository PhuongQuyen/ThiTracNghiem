using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ThiTracNghiem_BackEndAPI.Models;
using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.Roles;
using ThiTracNghiem_ViewModel.Users;

namespace ThiTracNghiem_BackEndAPI.Services.RoleServices
{
    public class RoleService : IRoleService
    {
        private readonly IConfiguration _configuration;
        private readonly tracnghiemContext _context;

        public RoleService(IConfiguration configuration, tracnghiemContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<ApiResult<bool>> Delete(int roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null) return new ApiResultErrors<bool>($"Can not find role with id: {roleId}");
            _context.Roles.Remove(role);
            var numRowChange = await _context.SaveChangesAsync();
            if (numRowChange > 0)
            {
                return new ApiResultSuccess<bool>();
            }
            return new ApiResultErrors<bool>("Can not detete");
        }

        public async Task<ApiResult<RoleViewModel>> GetById(int roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null)
            {
                return new ApiResultErrors<RoleViewModel>("not found");
            }
            var roleViewModel = new RoleViewModel()
            {
                Id = role.Id,
                Title = role.RoleTitle,
                Description = role.RoleDescription
            };
            return new ApiResultSuccess<RoleViewModel>(roleViewModel);
        }

        public async Task<DatatableResult<List<RoleViewModel>>> GetListRole(DatatableRequestBase request)
        {
            var query = from r in _context.Roles
                        select r;

            if (!String.IsNullOrEmpty(request.searchValue))
            {
                query = query.Where(x => x.RoleTitle.Contains(request.searchValue));
            }

            int totalRow = await query.CountAsync();

            if (request.sortColumnDirection == "desc")
            {
                switch (request.sortColumn)
                {
                    case "0": query = query.OrderByDescending(r => r.Id); break;
                    case "1": query = query.OrderByDescending(r => r.RoleTitle); break;
                    case "2": query = query.OrderByDescending(r => r.RoleDescription); break;
                }

            }
            else if (request.sortColumnDirection == "asc")
            {
                switch (request.sortColumn)
                {
                    case "0": query = query.OrderBy(r => r.Id); break;
                    case "1": query = query.OrderBy(r => r.RoleTitle); break;
                    case "2": query = query.OrderBy(r => r.RoleDescription); break;
                }

            }

            var data = await query.Skip(request.Skip).Take(request.PageSize)
                  .Select(x => new RoleViewModel()
                  {
                      Id = x.Id,
                      Title = x.RoleTitle,
                      Description = x.RoleDescription,
                      Action = String.Format("<a href = 'javascript:void(0)' data-toggle = 'tooltip' id = 'edit' data-id = '{0}' data-original-title='Edit' class='btn mr-5 btn-xs btn-warning btn-edit'><i class='glyphicon glyphicon-edit'></i> Sửa</a><a href = 'javascript:void(0)' data-toggle='tooltip' id='delete' data-id='{1}' data-original-title='Delete' class='btn btn-xs btn-danger btn-delete'><i class='glyphicon glyphicon-trash'></i> Xóa</a>", x.Id, x.Id)
                  }).ToListAsync();

            var result = new DatatableResult<List<RoleViewModel>>()
            {
                recordsTotal = totalRow,
                recordsFiltered = totalRow,
                Draw = request.Draw,
                Data = data
            };
            return result;
        }

        public async Task<ApiResult<string>> Create(RoleViewModel request)
        {
            var role = new Roles()
            {
                RoleTitle = request.Title,
                RoleDescription = request.Description
            };

            _context.Roles.Add(role);
            var numRowChange = await _context.SaveChangesAsync();
            if(numRowChange > 0)
            {
                return new ApiResultSuccess<string>("inserted");
            }
            else
            {
                return  new ApiResultErrors<string>("Faild");
            }

        }

        public async Task<ApiResult<bool>> Update(RoleViewModel request, int roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null)
            {
                return new ApiResultErrors<bool>("Not found");
            }
            role.RoleTitle = request.Title;
            role.RoleDescription = request.Description;

            var numRowChange = await _context.SaveChangesAsync();
            if (numRowChange > 0)
            {
                return new ApiResultSuccess<bool>();
            }
            else
            {
                return new ApiResultErrors<bool>("Faild");
            }
        }
    }
}
