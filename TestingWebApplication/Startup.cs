namespace TestingWebApplication
{
    using Data.Database;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using TestingWebApplication.Data.Database.Model;

    /// <summary>
    /// ����� ������������� ���-�������.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// ������������ ����������.
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="Startup"/>.
        /// </summary>
        /// <param name="config">������������ ����������.</param>
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// ��������� ������������ ���-�������.
        /// </summary>
        /// <param name="services">��������� IoC-����������.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(options =>
            {
                options.UseSqlite(_config.GetConnectionString("MainDb"));
                options.EnableSensitiveDataLogging();
            });
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddIdentity<UserDto, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 10;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<AppDbContext>();
        }

        /// <summary>
        /// ��������� �������������� ������������ �������.
        /// </summary>
        /// <param name="app">������ ����������.</param>
        /// <param name="env">���������� � ����� ����������.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
            app.UseStatusCodePages();
        }
    }
}
