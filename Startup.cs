using HMS.Interface;
using HMS.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddSession(options =>
            {
                options.Cookie.Name = "." + Configuration["AppSettings:Site"] + ".Session";
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });


            //Register dapper in scope  
            services.AddScoped<IDapperService, DapperService>();
            services.TryAddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IClinicMasterServices, ClinicMasterServices>();
            services.AddScoped<IDepartmentMasterServices, DepartmentMasterServices>();
            services.AddScoped<IDesignationMasterServices, DesignationMasterServices>();
            services.AddScoped<IUserMasterServices, UserMasterServices>();
            services.AddScoped<ICommonService, CommonService>();
            //services.AddScoped<IClinicDepartmentMasterServices, ClinicDepartmentMasterServices>();
            //services.AddScoped<IClinicDesignationMasterServices, ClinicDesignationMasterServices>();
            services.AddScoped<IPatientMasterServices, PatientMasterServices>();
            services.AddScoped<IConsultantMasterServices, ConsultantMasterServices>();
            services.AddScoped<IPatientGeneralDetailMasterServices, PatientGeneralDetailMasterServices>();
            services.AddScoped<IPatientConsultantMasterServices, PatientConsultantMasterServices>();
            services.AddScoped<IRevisitDetailMasterServices, RevisitDetailMasterServices>();
            services.AddScoped<IServiceHeadMasterService, ServiceHeadMasterService>();
            services.AddScoped<IServiceMasterService, ServiceMasterService>();
            services.AddScoped<IPatientServiceMasterServices, PatientServiceMasterServices>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
