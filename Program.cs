
var AllowedMethods =  new List<string>{"POST","PUT"}; 

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

builder.Configuration.AddEnvironmentVariables(prefix: "ASPNETCORE_");

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ProxySettings"));

var app = builder.Build();


app.MapReverseProxy(proxyPipeline =>
{
    proxyPipeline.Use(async (context, next) =>
    {
        if (AllowedMethods.Contains(context.Request.Method))
        {
            context.Request.EnableBuffering();
            var bodyAsText = new StreamReader(context.Request.Body).ReadToEndAsync();
            app.Logger.LogInformation("Body --> " + bodyAsText.Result);
            context.Request.Body.Position = 0;
        }
        await next.Invoke(context);
    });

});

app.Logger.LogInformation("Starting the app.......");

app.Run();
