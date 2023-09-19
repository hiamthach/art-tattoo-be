using Microsoft.AspNetCore.Mvc;

namespace art_tattoo_be.Controllers;

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

    return Ok("{ \"message\": \"pong\"}");
  }
}