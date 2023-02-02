using Serilog; 
using YarpDemo; 

var builder = WebApplication.CreateBuilder(args);

//add serilog to modified application log 
builder.Host.UseSerilog();

builder.Services.AddControllers();

var configuration = builder.Configuration
       .AddJsonFile($"Configs/appsettings.{Environment.GetEnvironmentVariable("YARP_ENVIRONMENT")}.json",true)
       .AddEnvironmentVariables(prefix: "YARP_")
       .Build();

builder.Services.AddReverseProxy()
    .LoadFromConfig(configuration.GetSection("ProxySettings"));

// services

builder.Services.AddSingleton(typeof(YarpDemo.FeatureOptionWrapper));

builder.Services.AddOptions<YarpDemo.FeatureOption>()
                .Bind(configuration.GetSection(nameof(YarpDemo.FeatureOption)))
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
