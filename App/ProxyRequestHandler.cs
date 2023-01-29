

namespace YarpDemo; 

public class ProxyRequestHandler {

    private List<string> AllowedMethods = new List<string> { "POST", "PUT" };
    private readonly ILogger<ProxyRequestHandler> _logger;
    public ProxyRequestHandler(ILogger<ProxyRequestHandler> logger ){
        _logger = logger; 
    }
    public async Task Handle(HttpContext context,RequestDelegate next)
    {
       {
        if (AllowedMethods.Contains(context.Request.Method))
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