using Microsoft.AspNetCore.Mvc;
using YarpDemo; 

namespace YarpDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class FeatureConfigurationController : ControllerBase
{ 
    private readonly ILogger<FeatureConfigurationController> _logger;
    private readonly FeatureOptionWrapper _featureConfiguration; 
    public FeatureConfigurationController(ILogger<FeatureConfigurationController> logger,
        FeatureOptionWrapper featureConfiguration)
    {
        _logger = logger;
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
