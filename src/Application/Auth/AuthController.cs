using Microsoft.AspNetCore.Mvc;

namespace art_tattoo_be.Application.Auth;

[Produces("application/json")]
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
  private readonly ILogger<AuthController> _logger;

  public AuthController(ILogger<AuthController> logger)
  {
    _logger = logger;
  }

  [HttpPost("login")]
  public IActionResult Login([FromBody] LoginReq req)
  {
    _logger.LogInformation("Login email:" + req.Email);

    LoginResp resp = new LoginResp
    {
      AccessToken = "AccessToken",
      RefreshToken = "RefreshToken"
    };

    return Ok(resp);
  }

  [HttpPost("register")]
  public IActionResult Register([FromBody] RegisterReq req)
  {
    _logger.LogInformation("Register");

    return Ok(req);
  }

  [HttpPost("refresh")]
  public IActionResult Refresh()
  {
    _logger.LogInformation("Refresh");

    return Ok("Refresh");
  }

  [HttpPost("logout")]
  public IActionResult Logout()
  {
    _logger.LogInformation("Logout");

    return Ok("Logout");
  }
}