namespace art_tattoo_be.Application.Controllers;

using art_tattoo_be.Application.DTOs.Studio;
using art_tattoo_be.Application.Shared;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Application.Shared.Handler;
using art_tattoo_be.Domain.Media;
using art_tattoo_be.Domain.Studio;
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

  private readonly IMapper _mapper;

  public StudioController(ILogger<StudioController> logger, ArtTattooDbContext dbContext, ICacheService cacheService, IMapper mapper)
  {
    _logger = logger;
    _cacheService = cacheService;
    _studioRepo = new StudioRepository(mapper, dbContext);
    _mapper = mapper;
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
            Id = Guid.NewGuid(),
            StudioId = studioMapped.Id,
            DayOfWeek = w.DayOfWeek,
            OpenAt = w.OpenAt,
            CloseAt = w.CloseAt
          };
        }).ToList();
      }

      var result = _studioRepo.Update(studioMapped);

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
}
