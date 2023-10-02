namespace art_tattoo_be.Application.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
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
using art_tattoo_be.Application.Middlewares;
using art_tattoo_be.Core.Mail;

[Produces("application/json")]
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
  private readonly ILogger<AuthController> _logger;
  private readonly IJwtService _jwtService;
  private readonly ICacheService _cacheService;
  private readonly IMailService _mailService;
  private readonly IUserRepository _userRepo;

  public AuthController(ILogger<AuthController> logger, IJwtService jwtService, ArtTattooDbContext dbContext, ICacheService cacheService, IMailService mailService)
  {
    _logger = logger;
    _jwtService = jwtService;
    _userRepo = new UserRepository(dbContext);
    _cacheService = cacheService;
    _mailService = mailService;
  }

  [Protected]
  [HttpGet("session")]
  public IActionResult GetSession()
  {
    _logger.LogInformation("GetSession");
    try
    {
      var payload = HttpContext.Items["payload"] as Payload;

      return Ok(payload);
    }
    catch (Exception e)
    {
      _logger.LogError(e.Message);
      return ErrorResp.SomethingWrong(e.Message);
    }
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
      var refreshTk = GenerateRefreshTk();

      // create a redis refreshToken key
      var redisRftKey = "rft:" + refreshTk;
      await _cacheService.Set(redisRftKey, new RedisSession { UserId = user.Id, SessionId = sessionId }, TimeSpan.FromSeconds(JwtConst.REFRESH_TOKEN_EXP));

      // create a session Id key
      var redisKey = "ss:" + user.Id.ToString() + ":" + sessionId.ToString();
      await _cacheService.Set(redisKey, new RedisSession
      {
        UserId = user.Id,
        SessionId = sessionId,
        Refresh = refreshTk,
      }, TimeSpan.FromSeconds(JwtConst.REFRESH_TOKEN_EXP));

      TokenResp tokenResp = new()
      {
        AccessToken = accessTk,
        RefreshToken = refreshTk,
        AccessTokenExp = DateTimeOffset.UtcNow.AddSeconds(JwtConst.ACCESS_TOKEN_EXP).ToUnixTimeSeconds(),
        RefreshTokenExp = DateTimeOffset.UtcNow.AddSeconds(JwtConst.REFRESH_TOKEN_EXP).ToUnixTimeSeconds()
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
      // check email exist
      var userExist = _userRepo.GetUserByEmail(req.Email);
      if (userExist != null)
      {
        return ErrorResp.BadRequest("Email already exist");
      }

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
  public async Task<IActionResult> Refresh([FromBody] RefreshReq req)
  {
    _logger.LogInformation("Refresh");

    try
    {
      // Get the refresh token from the request body, and get user id and session id from redis
      // If not found, return error
      var refreshTk = req.RefreshToken;

      var redisRftKey = "rft:" + refreshTk;

      var rft = await _cacheService.Get<RedisSession>(redisRftKey);
      if (rft == null)
      {
        return ErrorResp.Unauthorized("Invalid token");
      }

      var ssId = rft.SessionId;
      var userId = rft.UserId;

      // Get the session of user from redis -> This session use to check if the refresh token is valid and force logout
      // If not found, return error
      var redisKey = "ss:" + userId + ":" + ssId.ToString();

      var ss = await _cacheService.Get<RedisSession>(redisKey);
      if (ss == null)
      {
        return ErrorResp.Unauthorized("Invalid token");
      }

      if (ss.Refresh != refreshTk)
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
        AccessTokenExp = DateTimeOffset.UtcNow.AddSeconds(JwtConst.ACCESS_TOKEN_EXP).ToUnixTimeSeconds(),
        RefreshToken = refreshTk,
        RefreshTokenExp = -1
      });
    }
    catch (Exception e)
    {
      return ErrorResp.Unauthorized(e.Message);
    }
  }

  [HttpPost("forget-password")]
  public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordReq req)
  {
    _logger.LogInformation("ForgetPassword");

    try
    {
      await _mailService.SendEmailAsync(req.Email, "Reset password", "Your reset password code is: 123456");

      return Ok(new BaseResp { Message = "Send email successfully", Success = true });
    }
    catch (Exception e)
    {
      _logger.LogError(e.Message);
      return ErrorResp.UnknownError(e.Message);
    }
  }


  [HttpPost("logout")]
  public async Task<IActionResult> Logout([FromBody] LogoutReq req)
  {
    _logger.LogInformation("Logout");

    try
    {
      var refreshTk = req.RefreshToken;

      var redisRftKey = "rft:" + refreshTk;

      var rft = await _cacheService.Get<RedisSession>(redisRftKey);
      if (rft == null)
      {
        return ErrorResp.Unauthorized("Invalid token");
      }

      var ssId = rft.SessionId;
      var userId = rft.UserId;

      var redisKey = "ss:" + userId + ":" + ssId.ToString();

      var ss = await _cacheService.Get<RedisSession>(redisKey);
      if (ss == null)
      {
        return ErrorResp.Unauthorized("Invalid token");
      }

      if (ss.Refresh != refreshTk)
      {
        return ErrorResp.Unauthorized("Invalid token");
      }

      await _cacheService.Remove(redisKey);
      await _cacheService.Remove(redisRftKey);

      return Ok(new BaseResp { Message = "Logout successfully", Success = true });
    }
    catch (Exception e)
    {
      return ErrorResp.Unauthorized(e.Message);
    }
  }

  private string GenerateAccessTk(Guid userId, Guid sessionId, int roleId)
  {
    return _jwtService.GenerateToken(userId, sessionId, roleId, JwtConst.ACCESS_TOKEN_EXP);
  }

  private string GenerateRefreshTk()
  {
    var randomNumber = new byte[64];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);
    return Convert.ToBase64String(randomNumber);
  }
}
