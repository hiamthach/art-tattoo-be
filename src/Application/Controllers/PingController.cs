namespace art_tattoo_be.Application.Controllers;

using art_tattoo_be.Infrastructure.Cache;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("ping")]
public class PingController : ControllerBase
{
  private readonly ILogger<PingController> _logger;
  private readonly ICacheService _cacheService;

  public PingController(ILogger<PingController> logger, ICacheService cacheService)
  {
    _logger = logger;
    _cacheService = cacheService;
  }

  [HttpGet(Name = "GetPing")]
  public IActionResult Get()
  {
    _logger.LogInformation("Ping");

    var appVer = Environment.GetEnvironmentVariable("APP_VERSION");

    return Ok(new { message = "pong pong", appVersion = appVer });
  }

  [HttpDelete("clear-cache")]
  public IActionResult ClearCache()
  {
    _logger.LogInformation("Clear cache");

    _cacheService.Clear();

    return Ok(new { message = "clear cache" });
  }
}
