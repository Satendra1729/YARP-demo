
using Microsoft.Extensions.Options; 
namespace YarpDemo; 

public class ProxyRequestLogger{


    // Default allowed method for request logging are POST and PUT
    private readonly ILogger<ProxyRequestLogger> _logger;
    private readonly FeatureOptionWrapper _featureConfiguration; 
    public ProxyRequestLogger(ILogger<ProxyRequestLogger> logger, 
    FeatureOptionWrapper featureConfiguration ){
        _logger = logger; 
        _featureConfiguration = featureConfiguration; 
    }
    public async Task Handle(HttpContext context,RequestDelegate next)
    {
       {
        if (_featureConfiguration.AllowedMethod.Select(x=>x.ToLower()).Contains(context.Request.Method.ToLower()) && 
           _featureConfiguration.IsRequestLoggingEnabled)
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
        await next.Invoke(context);
    }
    }
}