
var AllowedMethods = new List<string> { "POST", "PUT" };

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

var app = builder.Build();

// Read target Enviorment appsettings using YARP_ENVIRONMENT, If not then read only appsettings.json
// enabling the logging for POST and PUT request
app.Map("/enable",() => "enabled"); 

// disable the logging for POST and PUT request
app.Map("/disable",() => "disabled"); 

app.MapReverseProxy(proxyPipeline =>
{
    proxyPipeline.Use(async (context, next) =>
    {
        if (AllowedMethods.Contains(context.Request.Method))
        {
            context.Request.EnableBuffering();
            // sepressing the Error 
            try
            {
                var bodyAsText = new StreamReader(context.Request.Body).ReadToEndAsync();
                app.Logger.LogInformation("Body --> " + bodyAsText.Result);
            }
            catch (Exception ex)
            {
                // Logging on Error
                app.Logger.LogError(ex.Message);
            }
            finally
            {
                // Resetting the Postion of body streme to 0 regardless of error or normal scenario
                context.Request.Body.Position = 0;
            }
        }
        await next.Invoke(context);
    });

});

app.Logger.LogInformation("Starting the app.......");

app.Run();
