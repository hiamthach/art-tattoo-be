namespace art_tattoo_be.Application.Controllers;

using art_tattoo_be.Application.DTOs.Studio;
using art_tattoo_be.Application.Middlewares;
using art_tattoo_be.Application.Shared;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Application.Shared.Handler;
using art_tattoo_be.Core.Jwt;
using art_tattoo_be.Domain.Media;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Domain.User;
using art_tattoo_be.Infrastructure.Cache;
using art_tattoo_be.Infrastructure.Database;
using art_tattoo_be.Infrastructure.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

[Produces("application/json")]
[ApiController]
[Route("api/studios")]
public class StudioController : ControllerBase
{
  private readonly ILogger<StudioController> _logger;
  private readonly ICacheService _cacheService;
  private readonly IStudioRepository _studioRepo;
  private readonly IUserRepository _userRepo;
  private readonly IMapper _mapper;

  public StudioController(ILogger<StudioController> logger, ArtTattooDbContext dbContext, ICacheService cacheService, IMapper mapper)
  {
    _logger = logger;
    _cacheService = cacheService;
    _studioRepo = new StudioRepository(dbContext);
    _userRepo = new UserRepository(dbContext);
    _mapper = mapper;
  }

  [HttpGet("status")]
  public async Task<IActionResult> GetStudioStatus()
  {
    _logger.LogInformation("GetStudioStatus");

    try
    {
      var redisKey = "studio-status";
      var cached = await _cacheService.Get<Dictionary<int, string>>(redisKey);
      if (cached != null)
      {
        return Ok(cached);
      }

      var statuses = Enum.GetValues<StudioStatusEnum>();

      var statusDict = new Dictionary<int, string>();

      foreach (var status in statuses)
      {
        statusDict.Add((int)status, status.ToString());
      }

      await _cacheService.Set(redisKey, statusDict, TimeSpan.FromDays(1));

      return Ok(statusDict);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [HttpPost()]
  public async Task<IActionResult> GetStudios([FromBody] GetStudioQuery req)
  {
    _logger.LogInformation("Get Studio");

    try
    {
      var redisKey = $"studios:{req.Page}:{req.PageSize}";
      if (req.ViewPortNE != null && req.ViewPortSW != null)
      {
        redisKey += $"?ne=[{req.ViewPortNE.Lat},{req.ViewPortNE.Lng}]&sw=[{req.ViewPortSW.Lat},{req.ViewPortSW.Lng}]";
      }

      if (req.SearchKeyword != null)
      {
        redisKey += $"?search={req.SearchKeyword}";
      }

      var studiosCache = await _cacheService.Get<StudioResp>(redisKey);

      if (studiosCache != null)
      {
        return Ok(studiosCache);
      }

      StudioResp resp = new()
      {
        Page = req.Page,
        PageSize = req.PageSize,
      };

      var studios = _studioRepo.GetStudioPages(req);
      resp.Total = studios.TotalCount;
      resp.Data = _mapper.Map<List<StudioDto>>(studios.Studios);

      // set to cache
      await _cacheService.Set(redisKey, resp);

      return Ok(resp);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetStudioById([FromRoute] Guid id)
  {
    _logger.LogInformation("Get Studio @req", id);
    try
    {
      var redisKey = $"studio:{id}";

      var studioCache = await _cacheService.Get<StudioDto>(redisKey);

      if (studioCache != null)
      {
        return Ok(studioCache);
      }

      var studio = await _studioRepo.GetAsync(id);

      if (studio == null)
      {
        return ErrorResp.NotFound("Studio Not found");
      }

      var studioDto = _mapper.Map<StudioDto>(studio);


      await _cacheService.Set(redisKey, studioDto);

      return Ok(studioDto);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_STUDIO)]
  [HttpPost("create")]
  public async Task<IActionResult> CreateStudio([FromBody] CreateStudioReq req)
  {
    _logger.LogInformation("Create Studio");

    try
    {
      var studio = new Studio
      {
        Id = Guid.NewGuid(),
        Status = StudioStatusEnum.Active,
      };
      studio = _mapper.Map(req, studio);

      studio.WorkingTimes = req.WorkingTimes.Select(w =>
      {
        return new StudioWorkingTime
        {
          Id = Guid.NewGuid(),
          StudioId = studio.Id,
          DayOfWeek = w.DayOfWeek,
          OpenAt = w.OpenAt,
          CloseAt = w.CloseAt
        };
      }).ToList();

      studio.ListMedia = req.ListMedia.Select(m =>
      {
        return new Media
        {
          Id = Guid.NewGuid(),
          Url = m.Url,
          Type = m.Type
        };
      }).ToList();

      var result = await _studioRepo.CreateAsync(studio);

      if (result > 0)
      {
        // clear cache
        await _cacheService.ClearWithPattern("studios");
        return CreatedAtAction(nameof(CreateStudio), new BaseResp
        {
          Success = true,
          Message = "Studio Created"
        });
      }
      else
      {
        return ErrorResp.BadRequest("Studio Create Fail");
      }
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_STUDIO)]
  [HttpPut("/activate/{id}")]
  public async Task<IActionResult> ActivateStudio([FromRoute] Guid id)
  {
    _logger.LogInformation("Activate Studio @id", id);

    try
    {
      var studio = await _studioRepo.GetAsync(id);

      if (studio == null)
      {
        return ErrorResp.NotFound("Studio Not found");
      }

      var result = _studioRepo.UpdateStudioStatus(id, StudioStatusEnum.Active);

      if (result > 0)
      {
        var redisKey = $"studio:{id}";
        await _cacheService.Remove(redisKey);
        await _cacheService.ClearWithPattern("studios");

        return Ok(new BaseResp
        {
          Message = "Activate studio successfully",
          Success = true
        });
      }
      else
      {
        return ErrorResp.BadRequest("Activate studio failed");
      }
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_STUDIO, PermissionSlugConst.MANAGE_OWNED_STUDIO)]
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateStudio([FromBody] UpdateStudioReq req, [FromRoute] Guid id)
  {
    _logger.LogInformation("Update Studio @id", id);

    try
    {
      var studio = await _studioRepo.GetAsync(id);

      if (studio == null)
      {
        return ErrorResp.NotFound("Studio Not found");
      }

      var studioMapped = _mapper.Map(req, studio);

      if (req.WorkingTimes != null)
      {
        studioMapped.WorkingTimes = req.WorkingTimes.Select(w =>
        {
          return new StudioWorkingTime
          {
            StudioId = studioMapped.Id,
            DayOfWeek = w.DayOfWeek,
            OpenAt = w.OpenAt,
            CloseAt = w.CloseAt
          };
        }).ToList();
      }

      var mediaList = new List<Media>();
      mediaList.AddRange(studioMapped.ListMedia);
      if (req.ListNewMedia != null)
      {
        var newMedia = req.ListNewMedia.Select(m =>
        {
          return new Media
          {
            Id = Guid.NewGuid(),
            Url = m.Url,
            Type = m.Type
          };
        }).ToList();

        mediaList.AddRange(newMedia);
      }

      if (req.ListRemoveMedia != null)
      {
        var removeMedia = studioMapped.ListMedia.Where(m => req.ListRemoveMedia.Contains(m.Id.ToString())).ToList();
        mediaList.RemoveAll(m => removeMedia.Select(m => m.Id).Contains(m.Id));
      }

      var result = _studioRepo.Update(studioMapped, mediaList);

      if (result > 0)
      {
        var redisKey = $"studio:{id}";
        await _cacheService.Remove(redisKey);
        await _cacheService.ClearWithPattern("studios");

        return Ok(new BaseResp
        {
          Message = "Update studio successfully",
          Success = true
        });
      }
      else
      {
        return ErrorResp.BadRequest("Update studio failed");
      }
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_STUDIO)]
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteStudio([FromRoute] Guid id)
  {
    _logger.LogInformation("Delete Studio @id", id);

    try
    {
      var studio = await _studioRepo.GetAsync(id);

      if (studio == null)
      {
        return ErrorResp.NotFound("Studio Not found");
      }

      var result = _studioRepo.UpdateStudioStatus(id, StudioStatusEnum.Inactive);

      if (result > 0)
      {
        var redisKey = $"studio:{id}";
        await _cacheService.Remove(redisKey);
        await _cacheService.ClearWithPattern("studios");

        return Ok(new BaseResp
        {
          Message = "Delete studio successfully",
          Success = true
        });
      }
      else
      {
        return ErrorResp.BadRequest("Delete studio failed");
      }
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [HttpGet("artists")]
  public async Task<IActionResult> GetStudioArtists([FromQuery] Guid studioId)
  {
    _logger.LogInformation("Get Studio Artists @req", studioId);
    try
    {
      var redisKey = $"studio-users:artists:{studioId}";

      var studioArtistsCache = await _cacheService.Get<List<StudioUserDto>>(redisKey);

      if (studioArtistsCache != null)
      {
        return Ok(studioArtistsCache);
      }

      var studioArtists = _studioRepo.GetStudioArtist(studioId);

      if (studioArtists == null)
      {
        return ErrorResp.NotFound("Studio Artists Not found");
      }

      var studioArtistsDto = _mapper.Map<List<StudioUserDto>>(studioArtists);

      await _cacheService.Set(redisKey, studioArtistsDto);

      return Ok(studioArtistsDto);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_STUDIO, PermissionSlugConst.MANAGE_OWNED_STUDIO)]
  [HttpGet("user")]
  public async Task<IActionResult> GetStudioUsers([FromQuery] GetStudioUserQuery query)
  {
    _logger.LogInformation("Get Studio Users @req", query.StudioId);
    try
    {
      if (HttpContext.Items["permission"] is string permission && permission == PermissionSlugConst.MANAGE_OWNED_STUDIO && HttpContext.Items["payload"] is Payload payload)
      {
        var isFromStudio = _studioRepo.IsStudioUserExist(payload.UserId, query.StudioId);

        if (!isFromStudio)
        {
          return ErrorResp.Forbidden("You don't have permission to access this studio");
        }
      }

      var redisKey = $"studio-users:{query.StudioId}:{query.Page}:{query.PageSize}";

      if (query.SearchKeyword != null)
      {
        redisKey += $"?search={query.SearchKeyword}";
      }

      var studioUsersCache = await _cacheService.Get<StudioUserResp>(redisKey);

      if (studioUsersCache != null)
      {
        return Ok(studioUsersCache);
      }

      var studioUsers = _studioRepo.GetStudioUsers(query);

      if (studioUsers == null)
      {
        return ErrorResp.NotFound("Studio Users Not found");
      }

      StudioUserResp resp = new()
      {
        Page = query.Page,
        PageSize = query.PageSize,
        Total = studioUsers.TotalCount,

        Data = _mapper.Map<List<StudioUserDto>>(studioUsers.Users)
      };

      await _cacheService.Set(redisKey, resp);

      return Ok(resp);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_STUDIO, PermissionSlugConst.MANAGE_OWNED_STUDIO)]
  [HttpGet("user/{id}")]
  public async Task<IActionResult> GetStudioUserById([FromRoute] Guid id)
  {
    _logger.LogInformation("Get Studio User @req", id);
    try
    {
      if (HttpContext.Items["permission"] is string permission && permission == PermissionSlugConst.MANAGE_OWNED_STUDIO && HttpContext.Items["payload"] is Payload payload)
      {
        var studioId = _studioRepo.GetStudioIdByUserId(payload.UserId);
        var isFromStudio = _studioRepo.IsStudioUserExist(payload.UserId, studioId);

        if (!isFromStudio)
        {
          return ErrorResp.Forbidden("You don't have permission to access this studio");
        }
      }
      var redisKey = $"studio-user:{id}";

      var studioUserCache = await _cacheService.Get<StudioUserDto>(redisKey);

      if (studioUserCache != null)
      {
        return Ok(studioUserCache);
      }

      var studioUser = _studioRepo.GetStudioUser(id);

      if (studioUser == null)
      {
        return ErrorResp.NotFound("Studio User Not found");
      }

      var studioUserDto = _mapper.Map<StudioUserDto>(studioUser);

      await _cacheService.Set(redisKey, studioUserDto);

      return Ok(studioUserDto);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_STUDIO, PermissionSlugConst.MANAGE_OWNED_STUDIO)]
  [HttpPost("create/user")]
  public async Task<IActionResult> CreateStudioUser([FromBody] CreateStudioUser req)
  {
    _logger.LogInformation("Create Studio User");
    if (HttpContext.Items["permission"] is string permission && permission == PermissionSlugConst.MANAGE_OWNED_STUDIO && HttpContext.Items["payload"] is Payload payload)
    {
      var isFromStudio = _studioRepo.IsStudioUserExist(payload.UserId, req.StudioId);

      if (!isFromStudio)
      {
        return ErrorResp.Forbidden("You don't have permission to access this studio");
      }
    }

    try
    {
      // check studio exist
      var studioExist = _studioRepo.IsExist(req.StudioId);

      if (!studioExist)
      {
        return ErrorResp.NotFound("Studio Not found");
      }

      // get user by email
      var user = await _userRepo.GetUserByEmailAsync(req.Email);
      if (user == null)
      {
        return ErrorResp.NotFound("User Not found");
      }

      // check studio user exist
      var studioUserExist = _studioRepo.IsStudioUserExist(user.Id);
      if (studioUserExist)
      {
        return ErrorResp.BadRequest("Studio User Exist");
      }

      var studioUser = new StudioUser
      {
        Id = Guid.NewGuid(),
        StudioId = req.StudioId,
        UserId = user.Id,
        IsDisabled = true
      };

      var result = await _studioRepo.CreateStudioUserAsync(studioUser, req.RoleId);

      if (result > 0)
      {
        // clear cache
        await _cacheService.ClearWithPattern("studio-users");
        return CreatedAtAction(nameof(CreateStudioUser), new BaseResp
        {
          Success = true,
          Message = "Studio User Created"
        });
      }
      else
      {
        return ErrorResp.SomethingWrong("Studio User Create Fail");
      }
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_STUDIO, PermissionSlugConst.MANAGE_OWNED_STUDIO)]
  [HttpPut("user/{id}")]
  public async Task<IActionResult> UpdateStudioUser([FromRoute] Guid id, [FromBody] UpdateStudioUserReq req)
  {
    _logger.LogInformation("Update Studio User Status @id", id);

    try
    {
      if (HttpContext.Items["payload"] is not Payload payload)
      {
        return ErrorResp.Unauthorized("Unauthorized");
      }

      if (HttpContext.Items["permission"] is string permission && permission == PermissionSlugConst.MANAGE_OWNED_STUDIO)
      {
        var selfStudioUser = _studioRepo.GetStudioUserByUserId(payload.UserId);

        if (selfStudioUser == null)
        {
          return ErrorResp.NotFound("Studio User Not found");
        }

        if (selfStudioUser.Id == id)
        {
          return ErrorResp.Forbidden("You don't have permission to update yourself");
        }

        if (payload.RoleId > req.RoleId)
        {
          return ErrorResp.Forbidden("You don't have permission to update this user");
        }
      }

      if (req.RoleId == null && req.IsDisabled == null)
      {
        return ErrorResp.BadRequest("Invalid request");
      }

      var result = _studioRepo.UpdateStudioUser(id, new UpdateStudioUserData
      {
        IsDisabled = req.IsDisabled ?? null,
        RoleId = req.RoleId ?? null,
        SelfRoleId = payload.RoleId
      });

      if (result > 0)
      {
        // clear cache
        await _cacheService.ClearWithPattern("studio-users");
        await _cacheService.ClearWithPattern("users");
        await _cacheService.Remove($"studio-user:{id}");
        await _cacheService.ForceLogout(payload.UserId);
        return Ok(new BaseResp
        {
          Message = "Update studio user successfully",
          Success = true
        });
      }
      else
      {
        return ErrorResp.BadRequest("Update studio user failed");
      }
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_STUDIO, PermissionSlugConst.MANAGE_OWNED_STUDIO)]
  [HttpDelete("user/{id}")]
  public async Task<IActionResult> DeleteStudioUser([FromRoute] Guid id)
  {
    _logger.LogInformation("Delete Studio User @id", id);

    try
    {
      var studioUser = _studioRepo.GetStudioUser(id);
      if (studioUser == null)
      {
        return ErrorResp.NotFound("Studio User Not found");
      }
      if (HttpContext.Items["permission"] is string permission && permission == PermissionSlugConst.MANAGE_OWNED_STUDIO && HttpContext.Items["payload"] is Payload payload)
      {
        var isFromStudio = _studioRepo.IsStudioUserExist(payload.UserId, studioUser.StudioId);

        if (!isFromStudio)
        {
          return ErrorResp.Forbidden("You don't have permission to access this studio");
        }
      }
      var result = _studioRepo.DeleteStudioUser(studioUser.UserId);

      if (result > 0)
      {
        // clear cache
        await _cacheService.ClearWithPattern("studio-users");
        return Ok(new BaseResp
        {
          Message = "Delete studio user successfully",
          Success = true
        });
      }
      else
      {
        return ErrorResp.BadRequest("Delete studio user failed");
      }
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }
}

