using Serilog; 
using YARP_Proxy; 

var builder = WebApplication.CreateBuilder(args);

//add serilog to modified application log 
builder.Host.UseSerilog();

builder.Services.AddControllers();

var configuration = builder.Configuration
       .AddJsonFile($"Configs/appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",true)
       .AddEnvironmentVariables(prefix: "ASPNETCORE_")
       .Build();

builder.Services.AddReverseProxy()
    .LoadFromConfig(configuration.GetSection("ProxySettings"));

// services

builder.Services.AddSingleton(typeof(FeatureOptionWrapper));

builder.Services.AddOptions<FeatureOption>()
                .Bind(configuration.GetSection(nameof(FeatureOption)))
                .ValidateDataAnnotations();

Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();



// app 

var app = builder.Build();

app.MapControllers();

app.MapReverseProxy(proxyPipeline =>
{
    proxyPipeline.UseRequestLogger();
});

Log.Logger.Information("Starting proxy.......");

app.Run();

public partial class Program { }
