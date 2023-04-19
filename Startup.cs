using EmployeeManagement.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement
{
    public class Startup
    {
        private IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;        
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(options =>
            options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                     .AddEntityFrameworkStores<AppDbContext>();

            services.AddMvc().AddXmlSerializerFormatters();
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();

           /* services.AddTransient<IEmployeeRepository, MockEmployeeRepository>();
            services.AddSingleton<IEmployeeRepository,MockEmployeeRepository>();
            services.AddScoped<IEmployeeRepository, MockEmployeeRepository>();*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");


                // - 0 is a placeholder that will handle a specific error number

                /* Because a redirect is issued, the URL in the address bar changes
                   Returns a status code(200) when actually an error occured which isn't 
                   semantically correct - Rather use "UseStatusCodePagesWithReExecute */

                //app.UseStatusCodePagesWithRedirects("/Error/{0}");

                /* Re-execute the pipeline and returns the original status code(404 for example)*/
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
           // app.UseMvcWithDefaultRoute();
            app.UseMvc(routes =>{
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
