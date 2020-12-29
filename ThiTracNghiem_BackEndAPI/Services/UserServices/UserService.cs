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

namespace ThiTracNghiem_BackEndAPI.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly tracnghiemContext _context;

        public UserService(IConfiguration configuration, tracnghiemContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<ApiResult<string>> Authencate(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return new ApiResultErrors<string>("Email or Password is incorrect");
            }
            else
            {
                var userToken = new UserViewModel
                {
                    Id = user.Id,
                    RoleId = user.RoleId,
                    Email = user.Email,
                };
                var token = GenerateJwtToken(userToken);
                return new ApiResultSuccess<string>(token);
            }
        }

        public async Task<ApiResult<bool>> Delete(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return new ApiResultErrors<bool>($"Can not find user with id: {userId}");
            _context.Users.Remove(user);
            var numRowChange = await _context.SaveChangesAsync();
            if (numRowChange > 0)
            {
                return new ApiResultSuccess<bool>();
            }
            return new ApiResultErrors<bool>("Can not detete");
        }

        public Task<ApiResult<UserViewModel>> GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<UserViewModel>> GetById(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if(user == null)
            {
                return new ApiResultErrors<UserViewModel>("not found");
            }
            var userViewModel = new UserViewModel()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                LastName = user.LastName,
                JoinDate = user.JoinDate,
                Id = user.Id,
                WorkPlace = user.WorkPlace,
                Gender = user.Gender,
                RoleId = user.RoleId
            };
            return new ApiResultSuccess<UserViewModel>(userViewModel);
        }

        public Task<ApiResult<UserViewModel>> GetByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task<DatatableResult<List<UserViewModel>>> GetListUser(DatatableRequestBase request)
        {
            var query = from u in _context.Users
                        join r in _context.Roles on u.RoleId equals r.Id
                        select new { r, u };

            if (!String.IsNullOrEmpty(request.searchValue))
            {
                query = query.Where(x => x.u.Email.Contains(request.searchValue));
            }

            int totalRow = await query.CountAsync();

            if (request.sortColumnDirection == "desc")
            {
                switch (request.sortColumn)
                {
                    case "0": query = query.OrderByDescending(r => r.u.Id); break;
                    case "1": query = query.OrderByDescending(r => r.u.Email); break;
                    case "2": query = query.OrderByDescending(r => r.u.FirstName); break;
                    case "3": query = query.OrderByDescending(r => r.u.LastName); break;
                    case "4": query = query.OrderByDescending(r => r.u.RoleId); break;
                    case "5": query = query.OrderByDescending(r => r.u.JoinDate); break;
                }
                
            }
            else if (request.sortColumnDirection == "asc")
            {
                switch (request.sortColumn)
                {
                    case "0": query = query.OrderBy(r => r.u.Id); break;
                    case "1": query = query.OrderBy(r => r.u.Email); break;
                    case "2": query = query.OrderBy(r => r.u.FirstName); break;
                    case "3": query = query.OrderBy(r => r.u.LastName); break;
                    case "4": query = query.OrderBy(r => r.u.RoleId); break;
                    case "5": query = query.OrderBy(r => r.u.JoinDate); break;
                }

            }

            var data = await query.Skip(request.Skip).Take(request.PageSize)
                  .Select(x => new UserViewModel()
                  {
                      Email = x.u.Email,
                      FirstName = x.u.FirstName,
                      PhoneNumber = x.u.PhoneNumber,
                      Address = x.u.Address,
                      LastName = x.u.LastName,
                      JoinDate = x.u.JoinDate,
                      Gender = x.u.Gender,
                      Id = x.u.Id,
                      RoleTitle = x.r.RoleTitle,
                      Action = String.Format("<a href = 'javascript:void(0)' data-toggle = 'tooltip' id = 'edit' data-id = '{0}' data-original-title='Edit' class='btn mr-5 btn-xs btn-warning btn-edit'><i class='glyphicon glyphicon-edit'></i> Sửa</a><a href = 'javascript:void(0)' data-toggle='tooltip' id='delete' data-id='{1}' data-original-title='Delete' class='btn btn-xs btn-danger btn-delete'><i class='glyphicon glyphicon-trash'></i> Xóa</a>", x.u.Id, x.u.Id)
                  }).ToListAsync();

            var result = new DatatableResult<List<UserViewModel>>()
            {
                recordsTotal = totalRow,
                recordsFiltered = totalRow,
                Draw = request.Draw,
                Data = data
            };
            return result;
        }

        public Task<ApiResult<string>> GetPasswordResetToken(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<string>> Register(RegisterRequest request)
        {
            var email = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email && u.Active == 1);
            if (email != null)
            {
                return new ApiResultErrors<string>("Email is already");
            }

            var user = new Users()
            {
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                FirstName = request.FirstName,
                LastName = request.LastName,
                WorkPlace = request.WorkPlace,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Gender = request.Gender,
                RoleId = request.RoleId,
                JoinDate = request.JoinDate,
                Active = 1
            };

            _context.Users.Add(user);
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

        public async Task<ApiResult<bool>> Update(RegisterRequest request, int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            var role = await _context.Roles.FindAsync(request.RoleId);
            if (user == null)
            {
                return new ApiResultErrors<bool>("Not found");
            }
            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Role= role;
            user.Gender = request.Gender;
            user.WorkPlace = request.WorkPlace;
            user.PhoneNumber = request.PhoneNumber;
            user.Address = request.Address;
            await _context.SaveChangesAsync();
            return new ApiResultSuccess<bool>();
        }

        private string GenerateJwtToken(UserViewModel user)
        {
            var claims = new[]
           {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["Tokens:Issuer"],
                _configuration["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
