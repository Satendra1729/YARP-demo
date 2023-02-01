
using YarpDemo; 

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

builder.Services.AddControllers();

builder.Configuration
       .AddJsonFile($"Configs/appsettings.{Environment.GetEnvironmentVariable("YARP_ENVIRONMENT")}.json",true)
       .AddEnvironmentVariables(prefix: "YARP_");

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ProxySettings"));


// services

builder.Services.AddSingleton(typeof(YarpDemo.FeatureOptionWrapper));

builder.Services.AddOptions<YarpDemo.FeatureOption>()
                .Bind(builder.Configuration.GetSection(nameof(YarpDemo.FeatureOption)))
                .ValidateDataAnnotations();

// app 

var app = builder.Build();

app.MapControllers();

app.MapReverseProxy(proxyPipeline =>
{
    proxyPipeline.UseRequestLogger();
});

app.Logger.LogInformation("Starting proxy.......");

app.Run();
