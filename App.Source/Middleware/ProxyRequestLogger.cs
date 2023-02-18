
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Context;
namespace YARP_Proxy;

public class RequestLogger
{


    // Default allowed method for request logging are POST and PUT
    private readonly Serilog.ILogger _logger = Log.ForContext<RequestLogger>();
    private readonly FeatureOptionWrapper _featureOptionWrapper;
    private readonly RequestDelegate _next;

    public RequestLogger(FeatureOptionWrapper featureOptionWrapper, RequestDelegate next)
    {
        _featureOptionWrapper = featureOptionWrapper;
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {

        if (_featureOptionWrapper.AllowedMethod.Select(x => x.ToLower()).Contains(context.Request.Method.ToLower()) &&
           _featureOptionWrapper.IsRequestLoggingEnabled)
        {
            context.Request.EnableBuffering();

            await _next.Invoke(context);

            context.Request.Body.Position = 0;
            try
            {
                var bodyAsText = new StreamReader(context.Request.Body).ReadToEndAsync();

                using (LogContext.PushProperty("cate", "REQ"))
                {
                    _logger.Information("Requesting For {path} with {body} with return status {resc}",
                                         context.Request.Path,bodyAsText.Result, context.Response.StatusCode);

                }
            }
            catch (Exception ex)
            {
                _logger.Error("{Message} {Exception}", ex.Message, (ex.StackTrace ?? "").Trim());
            }
        }
        else
            await _next.Invoke(context);

    }
}

public static class RequestLoggerMiddlewareExtenstion
{
    public static IApplicationBuilder UseRequestLogger(this IReverseProxyApplicationBuilder bulider)
    {
        return bulider.UseMiddleware<RequestLogger>();
    }
}