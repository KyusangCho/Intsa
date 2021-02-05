using Intsa.Areas.Identity;
using Intsa.Data;
using Intsa.Models.Boards;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Syncfusion.Blazor;
using BlazorDemos.Shared;
using System.IO;
using Syncfusion.Licensing;

namespace Intsa
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            if (File.Exists(System.IO.Directory.GetCurrentDirectory() + "/SyncfusionLicense.txt"))
            {
                string licenseKey = System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/SyncfusionLicense.txt");
                SyncfusionLicenseProvider.RegisterLicense(licenseKey);
            }
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
                .AddEntityFrameworkStores<ApplicationDbContext>();
            
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            
            services.AddSyncfusionBlazor();
            services.AddSyncfusionBlazor();

            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            services.AddScoped<SampleService>(); 
            
            AddDependencyInjectionContainerForBoards(services); 

        }

        /// <summary>
        /// �������� ���� ������ ���� ���� �ڵ� ���� ���� 
        /// </summary>
        /// <param name="services"></param>
        private void AddDependencyInjectionContainerForBoards(IServiceCollection services)
        {
            // Board > NoticeAppDbContext.cs Inject: New Dbcontext Add
            services.AddEntityFrameworkSqlServer().AddDbContext<NoticeAppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")); 
            }); 


            // INoticeRepositoryAsync Inject: DI�����̳ʿ� ����(�������丮) ���
            services.AddTransient<INoticeRepositoryAsync, NoticeRepositoryAsync>(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YOUR LICENSE KEY");

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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
