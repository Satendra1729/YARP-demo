
using Microsoft.Extensions.Options; 
using Serilog;
using Serilog.Context; 
namespace YARP_Proxy;

public class FeatureOptionWrapper
{
    private bool isRequestLoggingEnabled; 
    public bool IsRequestLoggingEnabled {get
    {
        return isRequestLoggingEnabled; 
    }
    set {
        _logger.Information("Set isRequestLoggingEnabled to "+value); 
        this.isRequestLoggingEnabled = value; 
    }}

    public List<string> AllowedMethod {get;init;}


    private Serilog.ILogger _logger = Log.ForContext<FeatureOptionWrapper>(); 

    // configuration loading at starting time
    public FeatureOptionWrapper(IConfiguration config, IOptions<FeatureOption> proxyFeature)
    {
        
        this.IsRequestLoggingEnabled = proxyFeature.Value.isRequestLoggingEnabled; 

        this.AllowedMethod = proxyFeature.Value.allowedMethod?? new List<string>(); 
    }

}