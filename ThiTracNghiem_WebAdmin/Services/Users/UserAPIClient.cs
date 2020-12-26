using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using ThiTracNghiem_WebAdmin.Commons;

namespace ThiTracNghiem_WebAdmin.Services.Users

{
    public class UserAPIClient :IUserAPIClient
    {

        protected readonly IHttpClientFactory _httpClientFactor;
        private readonly IConfiguration _configuration;
        protected HttpClient _client;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public UserAPIClient(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration) 
        {
            _httpClientFactor = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _client = _httpClientFactor.CreateClient();
            _configuration = configuration;
            var baseUrl = _configuration.GetSection(Constants.BackendUrlBase).Value;
            _client.BaseAddress = new Uri(baseUrl);
        }
        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/user/authenticate", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResult<string>>(result);
            }
            return JsonConvert.DeserializeObject<ApiResult<string>>(result);
        }

        public Task<ApiResult<UserViewModel>> getUserById(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<string>> Register(RegisterRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<string>> Delete(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<UserViewModel>> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<UserViewModel>> GetUserByUserName(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
