namespace art_tattoo_be.Application.Controllers;

using art_tattoo_be.Application.DTOs.User;
using art_tattoo_be.Application.Middlewares;
using art_tattoo_be.Application.Shared;
using art_tattoo_be.Application.Shared.Handler;
using art_tattoo_be.Core.Crypto;
using art_tattoo_be.Core.Jwt;
using art_tattoo_be.Domain.User;
using art_tattoo_be.Infrastructure.Cache;
using art_tattoo_be.Infrastructure.Database;
using art_tattoo_be.Infrastructure.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
  private readonly ILogger _logger;
  private readonly IMapper _mapper;
  private readonly IUserRepository _userRepo;
  private readonly ICacheService _cacheService;

  public UserController(ILogger<UserController> logger, ArtTattooDbContext dbContext, IMapper mapper, ICacheService cacheService)
  {
    _logger = logger;
    _userRepo = new UserRepository(dbContext);
    _mapper = mapper;
    _cacheService = cacheService;
  }

  [Protected]
  [HttpGet("profile")]
  public async Task<IActionResult> GetProfile()
  {
    if (HttpContext.Items["payload"] is not Payload payload)
    {
      return ErrorResp.Unauthorized("Unauthorized");
    }

    _logger.LogInformation($"Get User {payload.UserId}");

    try
    {
      var redisKey = $"user:{payload.UserId}";

      var userCache = await _cacheService.Get<UserDto>(redisKey);
      if (userCache != null)
      {
        return Ok(userCache);
      }

      var user = _userRepo.GetUserById(payload!.UserId);

      if (user == null)
      {
        return ErrorResp.NotFound("User not found");
      }

      var userDto = _mapper.Map<UserDto>(user);

      await _cacheService.Set(redisKey, userDto);

      return Ok(userDto);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [HttpPut("profile")]
  public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserReq req)
  {

    if (HttpContext.Items["payload"] is not Payload payload)
    {
      return ErrorResp.Unauthorized("Unauthorized");
    }

    _logger.LogInformation($"Update User {payload.UserId}");

    try
    {
      var user = _userRepo.GetUserById(payload.UserId);
      if (user == null)
      {
        return ErrorResp.NotFound("User not found");
      }

      var userMapped = _mapper.Map(req, user);

      var result = _userRepo.UpdateUser(userMapped);

      if (result > 0)
      {
        var redisKey = $"user:{payload.UserId}";
        await _cacheService.Remove(redisKey);

        return Ok(new BaseResp { Message = "Update user success", Success = true });
      }
      else
      {
        return ErrorResp.SomethingWrong("Update user failed");
      }
    }
    catch (Exception e)
    {
      _logger.LogError(e.Message);
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [HttpPut("password")]
  public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordReq req)
  {
    if (HttpContext.Items["payload"] is not Payload payload)
    {
      return ErrorResp.Unauthorized("Unauthorized");
    }

    _logger.LogInformation($"Update User {payload.UserId}");

    try
    {
      var user = _userRepo.GetUserById(payload.UserId);
      if (user == null)
      {
        return ErrorResp.NotFound("User not found");
      }

      if (!CryptoService.VerifyPassword(req.OldPassword, user.Password))
      {
        return ErrorResp.Unauthorized("Old password is not correct");
      }

      var hashedPass = CryptoService.HashPassword(req.NewPassword);

      var result = _userRepo.UpdateUserPassword(payload.UserId, hashedPass);

      if (result > 0)
      {
        var redisKey = $"user:{payload.UserId}";
        await _cacheService.Remove(redisKey);
        await _cacheService.ForceLogout(payload.UserId);

        return Ok(new BaseResp { Message = "Update password success", Success = true });
      }
      else
      {
        return ErrorResp.SomethingWrong("Update password failed");
      }
    }
    catch (Exception e)
    {
      _logger.LogError(e.Message);
      return ErrorResp.SomethingWrong(e.Message);
    }
  }
}
