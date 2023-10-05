namespace art_tattoo_be.Application.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("ping")]
public class PingController : ControllerBase
{
  private readonly ILogger<PingController> _logger;

  public PingController(ILogger<PingController> logger)
  {
    _logger = logger;
  }

  [HttpGet(Name = "GetPing")]
  public IActionResult Get()
  {
    _logger.LogInformation("Ping");

    var appVer = Environment.GetEnvironmentVariable("APP_VERSION");

    return Ok(new { message = "pong pong", appVersion = appVer });
  }
}
