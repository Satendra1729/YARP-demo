

namespace YarpDemo;
public class RuntimeConfiguration
{
    public bool enable {get;set;}

    // configuration loading at starting time
    public RuntimeConfiguration(IConfiguration config)
    {
        this.enable = config.GetSection("enableLogging").Get<bool>(); 
    }
}