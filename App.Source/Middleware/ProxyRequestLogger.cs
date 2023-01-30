

namespace YarpDemo; 

public class ProxyRequestLogger{

    private List<string> AllowedMethods = new List<string> { "POST", "PUT" };
    private readonly ILogger<ProxyRequestLogger> _logger;

    private readonly RuntimeConfiguration _runtimeConfiguration; 
    public ProxyRequestLogger(ILogger<ProxyRequestLogger> logger, 
    RuntimeConfiguration runtimeConfiguration ){
        _logger = logger; 
        _runtimeConfiguration = runtimeConfiguration; 
    }
    public async Task Handle(HttpContext context,RequestDelegate next)
    {
       {
        if (AllowedMethods.Contains(context.Request.Method) && _runtimeConfiguration.enable)
        {
            context.Request.EnableBuffering();
            // sepressing the Error 
            try
            {
                var bodyAsText = new StreamReader(context.Request.Body).ReadToEndAsync();
                _logger.LogInformation("Body --> " + bodyAsText.Result);
            }
            catch (Exception ex)
            {
                // Logging on Error
                _logger.LogError(ex.Message);
            }
            finally
            {
                // Resetting the Postion of body streme to 0 regardless of error or normal scenario
                context.Request.Body.Position = 0;
            }
        }
        await next.Invoke(context);
    }
    }
}