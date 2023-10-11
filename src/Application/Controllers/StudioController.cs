namespace art_tattoo_be.Application.Controllers;

using art_tattoo_be.Application.DTOs.Pagination;
using art_tattoo_be.Application.DTOs.Studio;
using art_tattoo_be.Application.Shared;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Application.Shared.Handler;
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

  [HttpGet]
  public async Task<IActionResult> GetStudios([FromQuery] PaginationReq req)
  {
    _logger.LogInformation("Get Studio");

    try
    {
      var redisKey = $"studios:{req.Page}:{req.PageSize}";

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

      var count = _studioRepo.Count();
      if (count == 0)
      {
        resp.Total = 0;

        return Ok(resp);
      }

      var studios = _studioRepo.GetStudioPages(req);
      resp.Total = count;
      resp.Data = _mapper.Map<List<StudioDto>>(studios);

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

  [HttpPost]
  public async Task<IActionResult> CreateStudio([FromBody] CreateStudioReq req)
  {
    _logger.LogInformation("Create Studio");

    try
    {
      var studio = new Studio
      {
        Id = Guid.NewGuid(),
        Name = req.Name,
        Detail = req.Detail,
        Logo = req.Logo,
        Address = req.Address,
        Latitude = req.Latitude,
        Longitude = req.Longitude,
        Website = req.Website,
        Instagram = req.Instagram,
        Facebook = req.Facebook,
        Phone = req.Phone,
        Status = StudioStatusEnum.Active,
      };

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
}
