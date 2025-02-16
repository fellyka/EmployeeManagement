//This line creates a WebApplicationBuilder instance used to configure the web application.
//It sets ups the environment for building the web application
var builder = WebApplication.CreateBuilder(args);

// This line builds the web application using the configuration provided by the builder
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// This line starts the web application and begins listening for incoming requests.
app.Run();
