//This line creates a WebApplicationBuilder instance used to configure the web application.
//It sets ups the environment for building the web application
using System.Runtime.Intrinsics.X86;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// This line adds MVC services to the dependency injection container.
builder.Services.AddControllersWithViews();


// This line builds the web application using the configuration provided by the builder
var app = builder.Build();

//Configure the HTTP request pipeline
if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Serve static files from the wwwroot folder
app.UseStaticFiles();

app.MapGet("/", () => "Hello World!");

// Map endpoints to controllers
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
);




// This line starts the web application and begins listening for incoming requests.
app.Run();
