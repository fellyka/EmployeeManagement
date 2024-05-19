using EmployeeManagement.Misc_Items;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace EmployeeManagement
{
    public class Program
    {
        /*Why does an Asp.net core have a Main() method?
           - An Asp .net core application initially starts as a Console application 
             and this Main method is the entry point into this application. This 
             Main method configures the application and starts it and at that point 
             it becomes an Asp .net Core web application.
        */
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);


            /*LogManager.LoadConfiguration(string) is obsolete: Replaced by LogManager.Setup()*/
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            //  LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

           /* builder.ConfigureLogging((hostingContext, loggingBuilder) =>
            {
                loggingBuilder.Configure(options =>
                {
                    options.ActivityTrackingOptions = ActivityTrackingOptions.SpanId
                                                        | ActivityTrackingOptions.TraceId
                                                        | ActivityTrackingOptions.ParentId;
                });
                loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
                loggingBuilder.AddEventSourceLogger();
            });*/


            /*Exceptionally DB Connection is done here*/
            var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
            builder.Services.AddDbContext<EmployeeDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), b => b.MigrationsAssembly("EmployeeManagement"));
            });
            /***************************************************************************************/
            /**************************************************************************************/

            /*Add identity service*/
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<EmployeeDbContext>();

            /*Add complexity to the password*/
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 3;
            });


            // Add services to the container.
            builder.Services.AddControllersWithViews(options =>
            {
                /*Let build an Authorisation policy, so that we don't have to use [Authorize] attribute on the controller*/
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                /*Add a filter*/
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();/*This line configures MVC and allows us to serialize our data in
                                                                                     Xml (not that important and can be removed)*/


            /* All depedency to be resolved here  */
            /*
              -AddSingleton: With a singleton, there is only a single instance. An instance is created, when the service is first requested and that
                             single instance is used by all http requests throughout the application.

              -AddTransient: With a transient service, a new instance is provided every time an instance is requested whether it is in the scope of 
                             the same htpp request or across different http requests.                        

              -AddScoped: With a scoped service, we get the same instance within the scope of a given http request but a new instance across different 
                          http requests.
             */
            //builder.Services.AddSingleton<IEmployeeRepository, MockEmployeeRepository>();
            builder.Services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();

            /*Reading my configurations */
            //  var config = builder.Services.Configure<MyKeySettings>(builder.Configuration.GetSection("MyKey")); --Wrong
            // builder.Configuration.AddJsonFile("appsettings.json");



            //********************************************************************************************************************************************
            //********************************************************************************************************************************************
            //********************************************************************************************************************************************
            //********************************************************************************************************************************************
            //********************************************************************************************************************************************


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                /*Make sure that youyou change your environment to something else, except for Development. This will respond with a generic message:
                  Status Code: 404; Not Found in your browser. For this reason, we rarely use the "UseStatusCodePages() middleware in a real world production
                  application
                */
                // app.UseStatusCodePages();

                /*We want to intercept the 404 and render our customized view to the user of the app. For that, we use :
                      UseStatusCodePagesWithRedirects  or the UseStatusCodePagesWithReExecute
                  From the end user standpoint, there is no difference in the behavior but from the software standpoint there is.
                  So, What is the difference between the 2? Which one is best to use?
                     -The problem with the "UseStatusCodePagesWithRedirects("/Error/{0}")" will first give a statusCode of 302 Found(whick means the request uri of the requested resource
                      has temporarily been changed to something else, which in this case is /Error/{0}. So, this middleware component issues another request immedialtely which passes again
                      through the process request line again. At the end, this middleware will redirect you to the error controller but won't acknowledge the error. For that reason, many
                      folks prefer using "UseStatusCodePagesWithReExecute("/Error/{0}")" which is a clever piece of a middleware. It reexecutes the pipeline and returns a 404 error in 
                      the network tab of your browser and prserve your url which is the correct behavior.

                When we use the UseStatusCodePagesWithReExecute(), we can retrieve the path that caused the error(Check the error controller)

                /*
                  The ...{0} is a place holder that automatically receive a non success status code. For example if we've a 404, the url will be 
                  /error/404. Ensure that you add the error controller
                */
                // app.UseStatusCodePagesWithRedirects("/Error/{0}");

                /*UseExceptionHandler("/Home/Error") is used to catch Global exception*/
                app.UseExceptionHandler("/Error");/*Global exception handler*/
                app.UseStatusCodePagesWithReExecute("/Error/{0}"); /*404 exception*/
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //  app.MapGet("/", () => "Hello World!");

            //Process used to run the application
            //app.MapGet("/", () => $"{System.Diagnostics.Process.GetCurrentProcess().ProcessName}");

            /*Reading my configuration*/


            app.MapControllerRoute(
               name: "default",
               pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
            //25
        }
    }
}
