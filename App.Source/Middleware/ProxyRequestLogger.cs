
using Microsoft.Extensions.Options; 
namespace YarpDemo; 

public class RequestLogger{


    // Default allowed method for request logging are POST and PUT
    private readonly ILogger<RequestLogger> _logger;
    private readonly FeatureOptionWrapper _featureOptionWrapper; 
    private readonly RequestDelegate _next;

    public RequestLogger(ILogger<RequestLogger> logger, 
    FeatureOptionWrapper featureOptionWrapper, RequestDelegate next ){
        _logger = logger; 
        _featureOptionWrapper = featureOptionWrapper; 
        _next = next; 
    }
    public async Task InvokeAsync(HttpContext context)
    {
       {
        if (_featureOptionWrapper.AllowedMethod.Select(x=>x.ToLower()).Contains(context.Request.Method.ToLower()) && 
           _featureOptionWrapper.IsRequestLoggingEnabled)
        {
            context.Request.EnableBuffering();
            try
            {
                var bodyAsText = new StreamReader(context.Request.Body).ReadToEndAsync();
                _logger.LogInformation("Body --> " + bodyAsText.Result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            finally
            {
                context.Request.Body.Position = 0;
            }
        }
        await _next.Invoke(context);
    }
    }
}

public static class RequestLoggerMiddlewareExtenstion {
    public static IApplicationBuilder UseRequestLogger( this IReverseProxyApplicationBuilder bulider)
    {
        return bulider.UseMiddleware<RequestLogger>(); 
    }
}