using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement
{
    public class Startup
    {
        private readonly IConfiguration config;

        public Startup(IConfiguration config)
        {
            this.config = config;
        }
        public void ConfigureServices(IServiceCollection services)
        {
           // services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddDbContextPool<AppDbContext>(sqlDb => sqlDb.UseSqlServer(config.GetConnectionString("EmpDbConnection")));

            //Using EF Core to retreive user and role in the underlying DB
            //This code add other tables in the DB, like users, claims, roles, etc
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                //Configure some of PasswordOptions class properties
                options.Password.RequiredLength = 10;
                options.Password.RequiredUniqueChars = 3;

            }).AddEntityFrameworkStores<AppDbContext>();

            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
          //  services.AddSingleton<IEmployeeRepository, MockEmployeeRepository>();


        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)//This method process the HttpResquest piple line
        {
            if (env.IsDevelopment()) //To use only during development
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error"); //To handle Unhandeled exception

                //  app.UseStatusCodePages(); -- Not so good for production environment

                //{0} is a placeholder that receive a non success status code : 404
               // app.UseStatusCodePagesWithRedirects("/Error/{0}");  -- returns a 200 in the background which is wrong

               //Clever peiece of middleware
                app.UseStatusCodePagesWithReExecute("/Error/{0}"); //--generate a status code 404, not a 200 ok and it preserves the original url

            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //CVonventional routing 
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
