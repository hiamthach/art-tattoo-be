namespace art_tattoo_be.Application.Controllers;

using Microsoft.AspNetCore.Mvc;
using art_tattoo_be.Application.DTOs.Auth;
using art_tattoo_be.Core.Jwt;
using art_tattoo_be.Application.Shared.Constant;
using art_tattoo_be.Infrastructure.Database;
using art_tattoo_be.Domain.User;
using art_tattoo_be.Infrastructure.Repository;
using art_tattoo_be.Application.Shared.Handler;
using art_tattoo_be.Infrastructure.Cache;

[Produces("application/json")]
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
  private readonly ILogger<AuthController> _logger;
  private readonly IJwtService _jwtService;
  private readonly ICacheService _cacheService;
  private readonly IUserRepository _userRepo;

  public AuthController(ILogger<AuthController> logger, IJwtService jwtService, ArtTattooDbContext dbContext, ICacheService cacheService)
  {
    _logger = logger;
    _jwtService = jwtService;
    _userRepo = new UserRepository(dbContext);
    _cacheService = cacheService;
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginReq req)
  {
    _logger.LogInformation("Login email:" + req.Email);
    try
    {

      if (req.Email == null || req.Password == null)
      {
        return ErrorResp.BadRequest("Email or password is null");
      }
      else
      {

        Guid sessionId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();

        var accessTk = GenerateAccessTk(userId, sessionId, 1);
        var refreshTk = GenerateRefreshTk(userId, sessionId, 1);

        await _cacheService.Set(sessionId.ToString(), refreshTk, TimeSpan.FromSeconds(JwtConst.REFRESH_TOKEN_EXP));

        TokenResp tokenResp = new()
        {
          AccessToken = accessTk,
          RefreshToken = refreshTk,
          AccessTokenExp = JwtConst.ACCESS_TOKEN_EXP,
          RefreshTokenExp = JwtConst.REFRESH_TOKEN_EXP
        };

        LoginResp resp = new()
        {
          Message = "Login successfully",
          Token = tokenResp
        };

        return Ok(resp);
      }
    }
    catch (Exception e)
    {
      _logger.LogError(e.Message);
      return ErrorResp.SomethingWrong(e.Message);
    }

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

    try
    {
      return Ok("Refresh");
    }
    catch (Exception e)
    {
      return ErrorResp.Unauthorized(e.Message);
    }
  }

  [HttpPost("logout")]
  public IActionResult Logout()
  {
    _logger.LogInformation("Logout");

    return Ok("Logout");
  }
  private string GenerateAccessTk(Guid userId, Guid sessionId, int roleId)
  {
    return _jwtService.GenerateToken(userId, sessionId, roleId, JwtConst.ACCESS_TOKEN_EXP);
  }

  private string GenerateRefreshTk(Guid userId, Guid sessionId, int roleId)
  {
    return _jwtService.GenerateToken(userId, sessionId, roleId, JwtConst.REFRESH_TOKEN_EXP);
  }
}
