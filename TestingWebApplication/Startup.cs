namespace TestingWebApplication
{
    using Data.Database;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

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
            app.UseMvcWithDefaultRoute();
            app.UseStatusCodePages();
        }
    }
}
