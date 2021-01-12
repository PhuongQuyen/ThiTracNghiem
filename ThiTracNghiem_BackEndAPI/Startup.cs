using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ThiTracNghiem_BackEndAPI.Models;
using ThiTracNghiem_BackEndAPI.Services.CategoriesServices;
using ThiTracNghiem_BackEndAPI.Services.ExamServices;
using ThiTracNghiem_BackEndAPI.Services.ModuleService;
using ThiTracNghiem_BackEndAPI.Services.QuestionServices;
using ThiTracNghiem_BackEndAPI.Services.RoleServices;
using ThiTracNghiem_BackEndAPI.Services.RoomService;
using ThiTracNghiem_BackEndAPI.Services.ScoreService;
using ThiTracNghiem_BackEndAPI.Services.UserServices;

namespace ThiTracNghiem_BackEndAPI
{
    public class Startup
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:5002").AllowAnyMethod().AllowAnyHeader();
                    });

            });
            services.AddDbContext<tracnghiemContext>(options => options.UseMySQL(Configuration.GetConnectionString("DbConnectionString")));

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IExamService, ExamService>();
            services.AddTransient<IQuestionService, QuestionService>();
            services.AddTransient<IRoomService, RoomService>();
            services.AddTransient<IModuleService, ModuleService>();
            services.AddTransient<ICategoriesServie, CategoriesService>();
            services.AddTransient<IScoreService, ScoreService>();
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
             );
            services.AddSingleton(_env.ContentRootFileProvider);
            string issuer = Configuration.GetValue<string>("Tokens:Issuer");
            string signingKey = Configuration.GetValue<string>("Tokens:Key");
            byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = issuer,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = System.TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
                };
            })
            .AddCookie();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
