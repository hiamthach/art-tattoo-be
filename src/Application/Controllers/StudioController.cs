namespace art_tattoo_be.Application.Controllers;

using art_tattoo_be.Application.DTOs.Pagination;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Infrastructure.Cache;
using art_tattoo_be.Infrastructure.Database;
using art_tattoo_be.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;

[Produces("application/json")]
[ApiController]
[Route("api/studio")]
public class StudioController : ControllerBase
{
  private readonly ILogger<StudioController> _logger;
  private readonly ICacheService _cacheService;
  private readonly IStudioRepository _studioRepo;

  public StudioController(ILogger<StudioController> logger, ArtTattooDbContext dbContext, ICacheService cacheService)
  {
    _logger = logger;
    _cacheService = cacheService;
    _studioRepo = new StudioRepository(dbContext);
  }

  [HttpGet]
  public IActionResult GetStudios([FromQuery] PaginationReq req)
  {
    _logger.LogInformation("Get Studio");
    return Ok("Get studio");
  }

}
