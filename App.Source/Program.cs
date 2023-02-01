

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

builder.Services.AddSingleton(typeof(YarpDemo.ProxyRequestLogger)); 

builder.Services.AddSingleton(typeof(YarpDemo.FeatureOptionWrapper));

builder.Services.AddOptions<YarpDemo.FeatureOption>()
                .Bind(builder.Configuration.GetSection(nameof(YarpDemo.FeatureOption)))
                .ValidateDataAnnotations();

// app 

var app = builder.Build();

app.MapControllers();

var proxyRequestHandler  = app.Services.GetRequiredService<YarpDemo.ProxyRequestLogger>(); 

app.MapReverseProxy(proxyPipeline =>
{
    proxyPipeline.Use(proxyRequestHandler.Handle);
});

app.Logger.LogInformation("Starting proxy.......");

app.Run();
