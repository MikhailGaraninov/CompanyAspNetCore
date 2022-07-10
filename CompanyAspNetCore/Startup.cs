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
        //���������� ����� ������������ � ����������
        public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; }
        public Startup(Microsoft.Extensions.Configuration.IConfiguration configuration) => Configuration = configuration;
        public void ConfigureServices(IServiceCollection services)
        {   //����������� ������ ����� Project �� appsettings.json
            //������� ������ appsettings � �������-�������� Config
            Configuration.Bind("Project", new Config());

            // ���������� ���������� ���������� � �������� ��������
            // AddTransient ������ ��� � ������ ������ http ������� ����� ���� ������� ������� ������ ��������(������������)
            services.AddTransient<ITextFieldsRepository, EFTextFieldsRepository>(); //��������� ��������� ITextFieldsRepository � ����������� ���������� Entity EFTextFieldsRepository
            services.AddTransient<IServiceItemsRepository, EFServiceItemsRepository>();
            services.AddTransient<DataManager>();

            //���������� �������� ��
            services.AddDbContext<AppDbContext>(x => x.UseSqlServer(Config.ConnectionString)); // ConnectionString �� appsettings

            // ����������� identity �������
            services.AddIdentity<IdentityUser, IdentityRole>(opts =>
                {
                    opts.User.RequireUniqueEmail = true;
                    opts.Password.RequiredLength = 6;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequireLowercase = false;
                    opts.Password.RequireUppercase = false;
                    opts.Password.RequireDigit = false;
                }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            // ����������� authentication coukie
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "myCompanyAuth";
                options.Cookie.HttpOnly = true; //���������� �� ��������� �������
                options.LoginPath = "/account/login"; //����� ��� ������ � ������ � ������ �������������� �������� �� ����� ���� ������� ���������� � ��������� � ��� login
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
            });

            //���������� ��������� ������������ � �������������(MVC)
            services.AddControllersWithViews()
                //���������� ������������� � asp.net core 3.0
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0).AddSessionStateTempDataProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            // !!! ������� ����������� moddleware ����� �����
            if (env.IsDevelopment())
            {
                //��������� ����� �� ������� � ����� ����������
                app.UseDeveloperExceptionPage();
            }

            //���������� ��������� ��������� ������ � ����������(css,js � �.�., � ����� wwwrout)
            app.UseStaticFiles();

            //���������� ������� �������������
            app.UseRouting();

            //���������� �������������� � �����������
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();


            //������������ ������ ��� ��������(���������)
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
