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
using art_tattoo_be.Domain.RoleBase;
using art_tattoo_be.Domain.Studio;

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
  private readonly IRoleBaseRepository _roleBaseRepo;
  private readonly IStudioRepository _studioRepo;

  public AuthController(ILogger<AuthController> logger, IJwtService jwtService, ArtTattooDbContext dbContext, ICacheService cacheService, IMailService mailService)
  {
    _logger = logger;
    _jwtService = jwtService;
    _userRepo = new UserRepository(dbContext);
    _roleBaseRepo = new RoleBaseRepository(dbContext);
    _studioRepo = new StudioRepository(dbContext);
    _cacheService = cacheService;
    _mailService = mailService;
  }

  [Protected]
  [HttpGet("session")]
  public async Task<IActionResult> GetSession()
  {
    _logger.LogInformation("GetSession");
    try
    {
      if (HttpContext.Items["payload"] is not Payload payload)
      {
        return ErrorResp.Unauthorized("Invalid token");
      }
      var permissionKey = $"roles:{payload.RoleId}:permissions";
      var permissions = await _cacheService.Get<List<string>>(permissionKey);
      if (permissions == null)
      {
        permissions = _roleBaseRepo.GetRolePermissionSlugs(payload.RoleId).ToList();
        await _cacheService.Set(permissionKey, permissions);
      }

      var studioUserKey = $"studio-user:{payload.UserId}:studio";
      var studio = await _cacheService.Get<string>(studioUserKey);

      if (studio != null && studio != Guid.Empty.ToString())
      {
        return Ok(new
        {
          UserId = payload.UserId,
          RoleId = payload.RoleId,
          SessionId = payload.SessionId,
          Status = payload.Status,
          Permissions = permissions,
          StudioId = studio
        });
      }
      else
      {
        var studioId = _studioRepo.GetStudioIdByUserId(payload.UserId);

        if (studioId != Guid.Empty)
        {
          await _cacheService.Set(studioUserKey, studioId);
          return Ok(new
          {
            UserId = payload.UserId,
            RoleId = payload.RoleId,
            SessionId = payload.SessionId,
            Permissions = permissions,
            StudioId = studioId
          });
        }
      }

      return Ok(new
      {
        UserId = payload.UserId,
        RoleId = payload.RoleId,
        SessionId = payload.SessionId,
        Permissions = permissions,
      });
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

      var accessTk = GenerateAccessTk(user.Id, sessionId, user.RoleId, user.Status);
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

  [HttpPost("require-verify")]
  public async Task<IActionResult> RequireVerify([FromBody] RequestCodeReq req)
  {
    _logger.LogInformation("RequireVerify");

    try
    {
      // Check if the email exist
      var user = await _userRepo.GetUserByEmailAsync(req.Email);
      if (user != null)
      {
        return ErrorResp.BadRequest("Email already exist");
      }

      // Create a reset password code
      var code = GenerateResetPasswordCode();
      // Save it to redis
      var redisKey = $"verify-email:{req.Email}";

      _ = _cacheService.Set(redisKey, code, TimeSpan.FromSeconds(60 * 5));

      // Send email to user
      await _mailService.SendEmailAsync(req.Email, "Mã xác thực", $"Mã xác thực của bạn là: {code}, mã sẽ hết hạn trong 5 phút");

      return Ok(new BaseResp { Message = "Send email successfully", Success = true });
    }
    catch (Exception e)
    {
      _logger.LogError(e.Message);
      return ErrorResp.UnknownError(e.Message);
    }
  }


  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] RegisterReq req)
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

      var redisKey = $"verify-email:{req.Email}";
      var code = await _cacheService.Get<string>(redisKey);

      if (code == null)
      {
        return ErrorResp.BadRequest("Invalid code");
      }

      if (code != req.VerifyCode)
      {
        return ErrorResp.BadRequest("Invalid code");
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

      var accessTk = GenerateAccessTk(user.Id, ssId, user.RoleId, user.Status);

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

  [HttpPost("request-code")]
  public async Task<IActionResult> RequestResetCode([FromBody] RequestCodeReq req)
  {
    _logger.LogInformation("RequestResetCode");

    try
    {
      // Check if the email exist
      var user = await _userRepo.GetUserByEmailAsync(req.Email);
      if (user == null)
      {
        return ErrorResp.NotFound("User not found");
      }

      // Create a reset password code
      var code = GenerateResetPasswordCode();
      // Save it to redis
      var redisKey = $"reset-password:{req.Email}";

      _ = _cacheService.Set(redisKey, code, TimeSpan.FromSeconds(60 * 5));

      // Send email to user
      await _mailService.SendEmailAsync(req.Email, "Mã xác nhận đổi mật khẩu", $"Mã xác nhận đổi mật khẩu của bạn là: {code}, mã sẽ hết hạn trong 5 phút");

      return Ok(new BaseResp { Message = "Send email successfully", Success = true });
    }
    catch (Exception e)
    {
      _logger.LogError(e.Message);
      return ErrorResp.UnknownError(e.Message);
    }
  }

  [HttpPost("reset-password")]
  public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordReq req)
  {
    _logger.LogInformation("ResetPassword");

    try
    {
      // Check if the code is valid
      var redisKey = $"reset-password:{req.Email}";
      var code = await _cacheService.Get<string>(redisKey);

      if (code == null)
      {
        return ErrorResp.BadRequest("Invalid code");
      }

      if (code != req.Code)
      {
        return ErrorResp.BadRequest("Invalid code");
      }

      // Update password
      var hashedPass = CryptoService.HashPassword(req.Password);

      var user = _userRepo.GetUserByEmail(req.Email);
      if (user == null)
      {
        return ErrorResp.NotFound("User not found");
      }

      user.Password = hashedPass;

      var result = _userRepo.UpdateUser(user);

      if (result > 0)
      {
        await _cacheService.ForceLogout(user.Id);
        return Ok(new BaseResp { Message = "Reset password successfully", Success = true });
      }
      else
      {
        return ErrorResp.BadRequest("Reset password failed");
      }
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

  private string GenerateAccessTk(Guid userId, Guid sessionId, int roleId, UserStatusEnum status)
  {
    return _jwtService.GenerateToken(userId, sessionId, roleId, status, JwtConst.ACCESS_TOKEN_EXP);
  }

  private static string GenerateRefreshTk()
  {
    var randomNumber = new byte[64];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);
    return Convert.ToBase64String(randomNumber);
  }

  private static string GenerateResetPasswordCode()
  {
    // Generate a random number 6 digits
    const string chars = "0123456789";
    Random random = new();
    char[] otpArray = new char[6];

    // Generate random characters for the OTP
    for (int i = 0; i < 6; i++)
    {
      otpArray[i] = chars[random.Next(chars.Length)];
    }

    // Convert the character array to a string
    string otp = new(otpArray);
    return otp;
  }
}

internal interface IStudioUserRepository
{
}

internal class StudioUserRepository
{
  private ArtTattooDbContext dbContext;

  public StudioUserRepository(ArtTattooDbContext dbContext)
  {
    this.dbContext = dbContext;
  }
}
