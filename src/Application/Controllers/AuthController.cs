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
using art_tattoo_be.Core.Crypto;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Application.Shared;
using Microsoft.AspNetCore.Authorization;
using art_tattoo_be.Application.Middlewares;

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
      // Check user exist
      var user = await _userRepo.GetUserByEmailAsync(req.Email);
      if (user == null)
      {
        return ErrorResp.NotFound("User not found");
      }

      // Check password
      if (!CryptoService.VerifyPassword(req.Password, user.Password))
      {
        return ErrorResp.Unauthorized("Wrong password");
      }

      Guid sessionId = Guid.NewGuid();

      var accessTk = GenerateAccessTk(user.Id, sessionId, user.RoleId);
      var refreshTk = GenerateRefreshTk(user.Id, sessionId, user.RoleId);

      var redisKey = "ss:" + sessionId.ToString();

      await _cacheService.Set(redisKey, refreshTk, TimeSpan.FromSeconds(JwtConst.REFRESH_TOKEN_EXP));

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
    try
    {
      var hashedPass = CryptoService.HashPassword(req.Password);

      var user = new User
      {
        Id = Guid.NewGuid(),
        Email = req.Email,
        Password = hashedPass,
        FullName = req.FullName,
        Phone = req.PhoneNumber,
        RoleId = RoleConst.GetRoleId(RoleConst.MEMBER),
        Status = UserStatusEnum.Active,
        Address = null,
        Avatar = null,
      };

      var result = _userRepo.CreateUser(user);

      if (result > 0)
      {
        return CreatedAtAction(nameof(Register), new BaseResp { Message = "Create successfully", Success = true });
      }
      else
      {
        return ErrorResp.BadRequest("Create failed");
      }
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Error creating user");
      return ErrorResp.UnknownError(e.Message);
    }
  }

  [HttpPost("refresh")]
  public async Task<IActionResult> Refresh()
  {
    _logger.LogInformation("Refresh");

    try
    {
      var refreshTk = Request.Headers["Authorization"].ToString().Split(" ")[1];
      var payload = _jwtService.ValidateToken(refreshTk);

      if (payload == null)
      {
        return ErrorResp.Unauthorized("Invalid token");
      }

      var ssId = payload.SessionId;
      var userId = payload.UserId;

      var redisKey = "ss:" + ssId.ToString();

      var ss = await _cacheService.Get<string>(redisKey);
      if (ss == null)
      {
        return ErrorResp.Unauthorized("Invalid token");
      }

      if (ss != refreshTk)
      {
        return ErrorResp.Unauthorized("Invalid token");
      }

      var user = _userRepo.GetUserById(userId);
      if (user == null)
      {
        return ErrorResp.NotFound("User not found");
      }

      var accessTk = GenerateAccessTk(user.Id, ssId, user.RoleId);

      return Ok(new TokenResp
      {
        AccessToken = accessTk,
        AccessTokenExp = JwtConst.ACCESS_TOKEN_EXP
      });
    }
    catch (Exception e)
    {
      return ErrorResp.Unauthorized(e.Message);
    }
  }

  [Protected]
  [HttpPost("logout")]
  public async Task<IActionResult> Logout()
  {
    _logger.LogInformation("Logout");

    try
    {
      if (HttpContext.Items["payload"] is not Payload payload)
      {
        return ErrorResp.BadRequest("Invalid token");
      }

      var ssId = payload.SessionId;

      var redisKey = "ss:" + ssId.ToString();

      await _cacheService.Remove(redisKey);
    }
    catch (Exception e)
    {
      return ErrorResp.BadRequest(e.Message);
    }

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
