using Microsoft.AspNetCore.Mvc;
using YarpDemo; 

namespace YarpDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class RuntimeConfigurationController : ControllerBase
{
 
    private readonly ILogger<RuntimeConfigurationController> _logger;

    private readonly RuntimeConfiguration _runtimeConfiguration; 

    public RuntimeConfigurationController(ILogger<RuntimeConfigurationController> logger,
        RuntimeConfiguration runtimeConfiguration)
    {
        _logger = logger;
        _runtimeConfiguration = runtimeConfiguration; 
    }

    [HttpGet]
    [Route("Enable")]
    public bool Enable()
    {
        _runtimeConfiguration.enable = true;
        return true; 
    }
    [HttpGet]
    [Route("Disable")]
    public bool Disable()
    {
        _runtimeConfiguration.enable = false;
        return true; 
    }
}
