namespace art_tattoo_be.src.Application.Controllers;

using art_tattoo_be.Application.Middlewares;
using art_tattoo_be.Application.Shared;
using art_tattoo_be.Application.Shared.Handler;
using art_tattoo_be.Core.Jwt;
using art_tattoo_be.Domain.Category;
using art_tattoo_be.Domain.Media;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Infrastructure.Cache;
using art_tattoo_be.Infrastructure.Database;
using art_tattoo_be.Infrastructure.Repository;
using art_tattoo_be.src.Application.DTOs.StudioService;
using art_tattoo_be.src.Domain.Studio;
using art_tattoo_be.src.Infrastructure.Repository;
using AutoMapper;

using Microsoft.AspNetCore.Mvc;

[Produces("application/json")]
[ApiController]
[Route("api/studio/service")]
public class StudioServiceController : ControllerBase
{
  private readonly ILogger<StudioServiceController> _logger;
  private readonly IStudioServiceRepository _stuServiceRepo;
  private readonly IStudioRepository _studioRepo;
  private readonly IMapper _mapper;
  private readonly ICategoryRepository _cateRepo;
  private readonly IStudioRepository _stuRepo;
  private readonly ICacheService _cacheService;

  public StudioServiceController(ILogger<StudioServiceController> logger, ArtTattooDbContext dbContext, IMapper mapper, ICacheService cacheService)
  {
    _logger = logger;
    _stuServiceRepo = new StudioServiceRepository(dbContext);
    _mapper = mapper;
    _cateRepo = new CategoryRepository(dbContext);
    _stuRepo = new StudioRepository(dbContext);
    _studioRepo = new StudioRepository(dbContext);
    _cacheService = cacheService;
  }

  [HttpGet]
  public async Task<IActionResult> GetAll([FromQuery] GetStudioServiceQuery query)
  {
    _logger.LogInformation("Get All Studio Service");
    try
    {
      var redisKey = $"studioServices:{query.StudioId}:{query.Page}:{query.PageSize}";
      if (query.SearchKeyword != null)
      {
        redisKey += $"?search={query.SearchKeyword}";
      }
      var studioServiceCache = await _cacheService.Get<StudioServiceResp>(redisKey);
      if (studioServiceCache != null)
      {
        return Ok(studioServiceCache);
      }

      StudioServiceResp resp = new()
      {
        Page = query.Page,
        PageSize = query.PageSize
      };

      var req = new GetStudioServiceReq
      {
        StudioId = query.StudioId,
        SearchKeyword = query.SearchKeyword ?? "",
        IsStudio = false,
        Page = query.Page,
        PageSize = query.PageSize
      };

      var studioServices = _stuServiceRepo.GetStudioServicePages(req);

      resp.Total = studioServices.TotalCount;

      resp.Data = _mapper.Map<List<StudioServiceDto>>(studioServices.StudioServices);

      await _cacheService.Set(redisKey, resp);

      return Ok(resp);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetById([FromRoute] Guid id)
  {
    try
    {
      var redisKey = $"studioService:{id}";
      var studioServiceCache = await _cacheService.Get<StudioServiceDto>(redisKey);

      if (studioServiceCache != null)
      {
        return Ok(studioServiceCache);
      }

      _logger.LogInformation("Get Studio Service by @id: ", id);
      var studioService = _stuServiceRepo.GetById(id);
      if (studioService == null)
      {
        return ErrorResp.NotFound("Studio Service not found");
      }

      if (studioService.IsDisabled)
      {
        return ErrorResp.NotFound("Studio Service not found");
      }

      var studioServiceDto = _mapper.Map<StudioServiceDto>(studioService);

      await _cacheService.Set(redisKey, studioServiceDto);
      return Ok(studioServiceDto);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_STUDIO_SERVICES, PermissionSlugConst.VIEW_STUDIO_SERVICES)]
  [HttpGet("owned")]
  public async Task<IActionResult> GetAllOwnServices([FromQuery] GetStudioServiceQuery query)
  {
    _logger.LogInformation("Get All Studio Service");
    try
    {
      var redisKey = $"studioServices:owned:{query.StudioId}:{query.Page}:{query.PageSize}";
      if (query.SearchKeyword != null)
      {
        redisKey += $"?search={query.SearchKeyword}";
      }
      var studioServiceCache = await _cacheService.Get<StudioServiceResp>(redisKey);
      if (studioServiceCache != null)
      {
        return Ok(studioServiceCache);
      }

      StudioServiceResp resp = new()
      {
        Page = query.Page,
        PageSize = query.PageSize
      };

      var req = new GetStudioServiceReq
      {
        StudioId = query.StudioId,
        SearchKeyword = query.SearchKeyword ?? "",
        IsStudio = true,
        Page = query.Page,
        PageSize = query.PageSize
      };

      var studioServices = _stuServiceRepo.GetStudioServicePages(req);

      resp.Total = studioServices.TotalCount;

      resp.Data = _mapper.Map<List<StudioServiceDto>>(studioServices.StudioServices);

      await _cacheService.Set(redisKey, resp);

      return Ok(resp);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_STUDIO_SERVICES, PermissionSlugConst.VIEW_STUDIO_SERVICES)]
  [HttpGet("owned/{id}")]
  public async Task<IActionResult> GetOwnServiceById([FromRoute] Guid id)
  {
    try
    {
      var redisKey = $"studioService:owned:{id}";
      var studioServiceCache = await _cacheService.Get<StudioServiceDto>(redisKey);

      if (studioServiceCache != null)
      {
        return Ok(studioServiceCache);
      }

      _logger.LogInformation("Get Studio Service by @id: ", id);
      var studioService = _stuServiceRepo.GetById(id);
      if (studioService == null)
      {
        return ErrorResp.NotFound("Studio Service not found");
      }

      var studioServiceDto = _mapper.Map<StudioServiceDto>(studioService);

      await _cacheService.Set(redisKey, studioServiceDto);
      return Ok(studioServiceDto);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_STUDIO_SERVICES)]
  [HttpPost("create")]
  public async Task<IActionResult> CreateStudioService([FromBody] CreateStudioServiceReq body)
  {
    _logger.LogInformation("Create Studio Service: @body", body);

    if (HttpContext.Items["payload"] is not Payload payload || HttpContext.Items["permission"] is not string permission)
    {
      return ErrorResp.Forbidden("You don't have permission to access this studio");
    }

    var studioId = _studioRepo.GetStudioIdByUserId(payload.UserId);

    if (studioId == Guid.Empty)
    {
      return ErrorResp.Forbidden("You don't have permission to access this studio");
    }

    try
    {
      var cate = _cateRepo.GetById(body.CategoryId);
      if (cate is null)
      {
        return ErrorResp.NotFound("Could not find Category");
      }

      var studioService = new StudioService
      {
        Id = Guid.NewGuid(),
        StudioId = studioId,
        CategoryId = body.CategoryId,
        Name = body.Name,
        Description = body.Description,
        MinPrice = body.MinPrice,
        MaxPrice = body.MaxPrice,
        Discount = body.Discount,
        ExpectDuration = body.ExpectDuration,
        Thumbnail = body.Thumbnail,
        IsDisabled = true,
        ListMedia = body.ListMedia.Select(m =>
        {
          return new Media
          {
            Id = Guid.NewGuid(),
            Url = m.Url,
            Type = m.Type
          };
        }).ToList()
      };

      var result = _stuServiceRepo.CreateStudioService(studioService);

      if (result > 0)
      {
        await _cacheService.ClearWithPattern("studioServices");

        return Ok(new BaseResp { Message = "Create successfully", Success = true });
      }
      else
      {
        return ErrorResp.BadRequest("Create failed");
      }
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_STUDIO_SERVICES)]
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteStudioService([FromRoute] Guid id)
  {
    _logger.LogInformation("Delete Studio Service Id: @id", id);

    if (HttpContext.Items["payload"] is not Payload payload || HttpContext.Items["permission"] is not string permission)
    {
      return ErrorResp.Forbidden("You don't have permission to access this studio");
    }

    var studioId = _studioRepo.GetStudioIdByUserId(payload.UserId);

    if (studioId == Guid.Empty)
    {
      return ErrorResp.Forbidden("You don't have permission to access this studio");
    }

    try
    {
      var studioService = _stuServiceRepo.GetById(id);

      if (studioService == null)
      {
        return ErrorResp.NotFound("Studio Service not found");
      }

      if (studioService.StudioId != studioId)
      {
        return ErrorResp.Forbidden("You don't have permission to access this studio");
      }

      var result = _stuServiceRepo.DeleteStudioService(id);
      if (result > 0)
      {
        var redisKey = $"studioService:{id}";
        await _cacheService.Remove(redisKey);
        await _cacheService.ClearWithPattern("studioServices");

        return Ok(new BaseResp { Message = "Delete Successfully", Success = true });
      }
      else
      {
        return ErrorResp.BadRequest("Delete failed");
      }
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Error delete Studio Service");
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_STUDIO_SERVICES)]
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateStudioService([FromBody] UpdateStudioServiceReq req, [FromRoute] Guid id)
  {
    _logger.LogInformation("Update Studio Service @id", id);
    if (HttpContext.Items["payload"] is not Payload payload || HttpContext.Items["permission"] is not string permission)
    {
      return ErrorResp.Forbidden("You don't have permission to access this studio");
    }

    var studioId = _studioRepo.GetStudioIdByUserId(payload.UserId);

    if (studioId == Guid.Empty)
    {
      return ErrorResp.Forbidden("You don't have permission to access this studio");
    }

    try
    {
      var studioService = _stuServiceRepo.GetById(id);
      if (studioService == null)
      {
        return ErrorResp.NotFound("Studio Service not found");
      }

      if (studioService.StudioId != studioId)
      {
        return ErrorResp.Forbidden("You don't have permission to access this studio");
      }

      if (req.CategoryId != null)
      {
        var cate = _cateRepo.GetById(req.CategoryId.Value);
        if (cate is null)
        {
          return ErrorResp.NotFound("Could not find Category");
        }
      }

      studioService.CategoryId = req.CategoryId ?? studioService.CategoryId;
      studioService.Name = req.Name ?? studioService.Name;
      studioService.Description = req.Description ?? studioService.Description;
      studioService.MinPrice = req.MinPrice != null ? req.MinPrice.Value : studioService.MinPrice;
      studioService.MaxPrice = req.MaxPrice != null ? req.MaxPrice.Value : studioService.MaxPrice;
      studioService.Discount = req.Discount != null ? req.Discount.Value : studioService.Discount;
      studioService.ExpectDuration = req.ExpectDuration ?? studioService.ExpectDuration;
      studioService.Thumbnail = req.Thumbnail ?? studioService.Thumbnail;
      studioService.IsDisabled = req.IsDisabled != null ? req.IsDisabled.Value : studioService.IsDisabled;

      var mediaList = new List<Media>();
      mediaList.AddRange(studioService.ListMedia);
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
        var removeMedia = studioService.ListMedia.Where(m => req.ListRemoveMedia.Contains(m.Id.ToString())).ToList();
        mediaList.RemoveAll(m => removeMedia.Select(m => m.Id).Contains(m.Id));
      }

      var result = _stuServiceRepo.UpdateStudioService(studioService, mediaList);

      if (result > 0)
      {
        var redisKey = $"studioService:{id}";
        await _cacheService.Remove(redisKey);
        await _cacheService.ClearWithPattern("studioServices");

        return Ok(new BaseResp
        {
          Message = "Update Studio Service successfully",
          Success = true
        });
      }
      else
      {
        return ErrorResp.BadRequest("Update Studio Service failed");
      }
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }
}

