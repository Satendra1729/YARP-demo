


var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

builder.Configuration
       .AddJsonFile("appsettings.json",true)
       .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("YARP_ENVIRONMENT")}",true)
       .AddEnvironmentVariables(prefix: "YARP_");

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ProxySettings"));

builder.Services.AddSingleton(typeof(YarpDemo.ProxyRequestHandler)); 

var app = builder.Build();

// Read target Enviorment appsettings using YARP_ENVIRONMENT, If not then read only appsettings.json
// enabling the logging for POST and PUT request
app.Map("/enable",() => "enabled"); 

// disable the logging for POST and PUT request
app.Map("/disable",() => "disabled"); 

var proxyRequestHandler  = app.Services.GetRequiredService<YarpDemo.ProxyRequestHandler>(); 

app.MapReverseProxy(proxyPipeline =>
{
    proxyPipeline.Use(proxyRequestHandler.Handle);
});

app.Logger.LogInformation("Starting the app.......");

app.Run();
