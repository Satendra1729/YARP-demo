
using System.ComponentModel.DataAnnotations;

namespace YarpDemo;

public class FeatureOption {
    [Required]
    public bool isRequestLoggingEnabled {get;set;}

    [RegularExpressionListAttribute("POST|PUT")]
    public List<string>? allowedMethod {get;set;}
}