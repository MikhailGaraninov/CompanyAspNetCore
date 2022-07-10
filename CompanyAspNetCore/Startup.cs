using CompanyAspNetCore.Domain;
using CompanyAspNetCore.Domain.Repositories.Abstract;
using CompanyAspNetCore.Domain.Repositories.EntityFramework;
using CompanyAspNetCore.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyAspNetCore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        //Добавление файла конфигурации в приложение
        public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; }
        public Startup(Microsoft.Extensions.Configuration.IConfiguration configuration) => Configuration = configuration;
        public void ConfigureServices(IServiceCollection services)
        {   //Подключение конфиг файла Project из appsettings.json
            //связали секцию appsettings с классом-оберткой Config
            Configuration.Bind("Project", new Config());

            // подключаем функционал приложения в качестве сервисов
            // AddTransient значит что в рамках одного http запроса может быть создано сколько угодно обьектов(репозиториев)
            services.AddTransient<ITextFieldsRepository, EFTextFieldsRepository>(); //связываем интерфейс ITextFieldsRepository с реализацией интерфейса Entity EFTextFieldsRepository
            services.AddTransient<IServiceItemsRepository, EFServiceItemsRepository>();
            services.AddTransient<DataManager>();

            //подключаем контекст БД
            services.AddDbContext<AppDbContext>(x => x.UseSqlServer(Config.ConnectionString)); // ConnectionString из appsettings

            // Настраиваем identity систему
            services.AddIdentity<IdentityUser, IdentityRole>(opts =>
                {
                    opts.User.RequireUniqueEmail = true;
                    opts.Password.RequiredLength = 6;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequireLowercase = false;
                    opts.Password.RequireUppercase = false;
                    opts.Password.RequireDigit = false;
                }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            // Настраиваем authentication coukie
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "myCompanyAuth";
                options.Cookie.HttpOnly = true; //недоступно на клиенской стороне
                options.LoginPath = "/account/login"; //чтобы был доступ у админа к панели администратора создадим по этому пути аккаунт контроллер с действием в нем login
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
            });

            //Добавление поддрежки контроллеров и представлений(MVC)
            services.AddControllersWithViews()
                //выставляем совместимость с asp.net core 3.0
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0).AddSessionStateTempDataProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            // !!! порядок регистрации moddleware очень важен
            if (env.IsDevelopment())
            {
                //подробный отчет по ошибкам в среде разработки
                app.UseDeveloperExceptionPage();
            }

            //подключаем поддержку статичных файлов в приложении(css,js и т.д., в папке wwwrout)
            app.UseStaticFiles();

            //добавление системы маршрутизации
            app.UseRouting();

            //подключаем аутентификацию и авторизацию
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();


            //регистрируем нужные нам маршруты(эндпоинты)
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
