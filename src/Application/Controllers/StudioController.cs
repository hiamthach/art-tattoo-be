namespace art_tattoo_be.Application.Controllers;

using art_tattoo_be.Application.DTOs.Pagination;
using art_tattoo_be.Application.DTOs.Studio;
using art_tattoo_be.Application.Shared.Handler;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Infrastructure.Cache;
using art_tattoo_be.Infrastructure.Database;
using art_tattoo_be.Infrastructure.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

[Produces("application/json")]
[ApiController]
[Route("api/studio")]
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
    _studioRepo = new StudioRepository(dbContext);
    _mapper = mapper;
  }

  [HttpGet]
  public async Task<IActionResult> GetStudios([FromQuery] PaginationReq req)
  {
    _logger.LogInformation("Get Studio");

    try
    {
      var redisKey = $"studios:{req.PageSize}:${req.Page}";

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

}
