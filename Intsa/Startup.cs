using Intsa.Areas.Identity;
using Intsa.Data;
using Intsa.Models.Boards;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Syncfusion.Blazor;
using BlazorDemos.Shared;
using System;
using Intsa.Services;
using Cafe.Shared;
using Intsa.Managers;
using Microsoft.AspNetCore.Authorization;
//using Intsa.Hubs;

namespace Intsa
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
            //if (File.Exists(System.IO.Directory.GetCurrentDirectory() + "/SyncfusionLicense.txt"))
            //{
            //    string licenseKey = System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/SyncfusionLicense.txt");
            //    Console.WriteLine(licenseKey);
            //    SyncfusionLicenseProvider.RegisterLicense(licenseKey);
            //}
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // [CORS][1] 사용등록
            // [CORS][1][2] 기본
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
            //services.AddResponseCompression(opts =>
            //{
            //    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
            //        new[] { "application/octet-stream" });
            //});

            services.ConfigureApplicationCookie(options =>
            {
                //options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                //options.Cookie.Name = "YourAppCookieName";
                //options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
                //options.LoginPath = "/Identity/Account/Login";
                //// ReturnUrlParameter requires 
                ////using Microsoft.AspNetCore.Authentication.Cookies;
                //options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                //options.SlidingExpiration = true;
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.MaxFailedAccessAttempts = 10;                           // 시도횟수 
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);      // 시간 
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 2;
            }); 


            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSyncfusionBlazor();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            services.AddScoped<IUserClaimsPrincipalFactory<IdentityUser>, AdditionalUserClaimsPrincipalFactory>();
            //services.AddSingleton<AuthorizationHandlerContext>(); 
            services.AddAuthorization(options =>
            {
                options.AddPolicy("TwoFactorEnabled",
                    x => x.RequireClaim("TwoFactorEnabled", "true")
                );
            });
            services.AddScoped<SampleService>(); 
            
            AddDependencyInjectionContainerForBoards(services);

            services.AddScoped<IFileUploadService, FileUploadService>();

            //services.AddTransient<IFileStorageManager, BlobStorageManager>();  // Cloud Upload
            services.AddTransient<IFileStorageManager, FileStorageManager>();  // Local Upload
        }

        /// <summary>
        /// DI for Board Repository Classes & Interfaces
        /// </summary>
        /// <param name="services"></param>
        private void AddDependencyInjectionContainerForBoards(IServiceCollection services)
        {
            // Board > NoticeAppDbContext.cs Inject: New Dbcontext Add
            services.AddEntityFrameworkSqlServer().AddDbContext<NoticeAppDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);  


            // INoticeRepositoryAsync Inject
            services.AddTransient<INoticeRepository, NoticeRepository>(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseResponseCompression(); 

            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDA2MDczQDMxMzgyZTM0MmUzMGp5RE1Md3RZM1FSZklhMHRMNmhyNi82MTJyU0x5dUxneHJ4R1hpMGJ4Zkk9");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("AllowAnyOrigin"); 

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                //endpoints.MapHub<NoticeHub>("/noticehub");      // 상단 공지사항 실시간 알림
                endpoints.MapFallbackToPage("/_Host");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
