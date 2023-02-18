using Microsoft.AspNetCore.Mvc;
using Serilog; 

namespace YARP_Proxy.Controllers;

[ApiController]
[Route("[controller]")]
public class FeatureConfigurationController : ControllerBase
{ 
    private readonly Serilog.ILogger _logger = Log.ForContext<FeatureConfigurationController>();
    private readonly FeatureOptionWrapper _featureConfiguration; 
    public FeatureConfigurationController(FeatureOptionWrapper featureConfiguration)
    {
        _featureConfiguration = featureConfiguration; 
    }

    [HttpGet]
    [Route("EnableLogging")]
    public bool EnableLogging()
    {
        _featureConfiguration.IsRequestLoggingEnabled = true;
        
        return true; 
    }
    [HttpGet]
    [Route("DisableLogging")]
    public bool DisableLogging()
    {
        _featureConfiguration.IsRequestLoggingEnabled = false;
        return true; 
    }

    [HttpGet]
    [Route("GetActiveFeature")]
    public FeatureOptionWrapper GetActiveFeature()
    {
        return _featureConfiguration;
        
    }

}
