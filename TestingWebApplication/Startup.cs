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
    /// Класс инициализации веб-сервиса.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Конфигурация приложения.
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Startup"/>.
        /// </summary>
        /// <param name="config">Конфигурация приложения.</param>
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Выполняет конфигурацию веб-сервиса.
        /// </summary>
        /// <param name="services">Экземпляр IoC-контейнера.</param>
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
        /// Выполняет дополнительную конфигурацию сервиса.
        /// </summary>
        /// <param name="app">Билдер приложения.</param>
        /// <param name="env">Информация о среде исполнения.</param>
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
