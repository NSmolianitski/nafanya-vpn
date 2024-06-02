using Microsoft.AspNetCore.Mvc;

namespace NafanyaVPN.Entities.Utility;

[Route("api/v1/checks")]
public class UtilityController(ILogger<UtilityController> logger) : Controller
{
    [HttpGet] 
    [Route("ping")] 
    public string Ping() => "pong";
    
    [HttpGet] 
    [Route("exception")] 
    public string Error()
    {
        try
        {
            throw new InvalidOperationException("Testing exception thrown");
        }
        catch (InvalidOperationException e)
        {
            logger.LogError("{Message}", e);
        }
        
        return "Ok";
    }
}