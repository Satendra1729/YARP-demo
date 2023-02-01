
using Microsoft.Extensions.Options; 
namespace YarpDemo;
public class FeatureOptionWrapper
{
    private bool isRequestLoggingEnabled; 
    public bool IsRequestLoggingEnabled {get
    {
        return isRequestLoggingEnabled; 
    }
    set {
        _logger.LogInformation("Set isRequestLoggingEnabled to "+value); 
        this.isRequestLoggingEnabled = value; 
    }}

    public List<string> AllowedMethod {get;init;}


    private ILogger<FeatureOptionWrapper> _logger {get;set;}

    // configuration loading at starting time
    public FeatureOptionWrapper(ILogger<FeatureOptionWrapper> logger,
                                IConfiguration config, IOptions<FeatureOption> proxyFeature)
    {
        _logger = logger; 
        
        this.IsRequestLoggingEnabled = proxyFeature.Value.isRequestLoggingEnabled; 

        this.AllowedMethod = proxyFeature.Value.allowedMethod?? new List<string>(); 
    }

}