using art_tattoo_be.Application.DTOs.Shift;
using art_tattoo_be.Application.Shared.Handler;
using art_tattoo_be.Domain.Booking;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Infrastructure.Cache;
using art_tattoo_be.Infrastructure.Database;
using art_tattoo_be.Infrastructure.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;

namespace art_tattoo_be.Application.Controllers;

[ApiController]
[Route("api/shift")]
public class ShiftController : ControllerBase
{
  private readonly ILogger<ShiftController> _logger;
  private readonly IShiftRepository _shiftRepo;
  private readonly IStudioRepository _studioRepo;

  private readonly IMapper _mapper;
  private readonly ICacheService _cacheService;

  public ShiftController(ILogger<ShiftController> logger, ArtTattooDbContext dbContext, IMapper mapper, ICacheService cacheService)
  {
    _shiftRepo = new ShiftRepository(dbContext);
    _logger = logger;
    _mapper = mapper;
    _cacheService = cacheService;
    _studioRepo = new StudioRepository(dbContext);
  }

  [HttpGet]
  public async Task<IActionResult> GetShifts([FromQuery] ShiftQuery query)
  {
    _logger.LogInformation("GetShifts");
    if (DateTime.Compare(query.End, query.Start) <= 0)
    {
      return ErrorResp.BadRequest("End date must be greater than start date");
    }

    try
    {
      var redisKey = $"shifts:{query.Start}:{query.End}";

      if (query.ArtistId != null)
      {
        redisKey += $":{query.ArtistId}";
      }

      if (query.StudioId != null)
      {
        redisKey += $":{query.StudioId}";
      }

      var cachedShifts = await _cacheService.Get<IEnumerable<Shift>>(redisKey);
      if (cachedShifts != null)
      {
        return Ok(cachedShifts);
      }

      var result = _shiftRepo.GetAllAsync(query);

      await _cacheService.Set(redisKey, result);
      return Ok(result);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetShiftById(Guid id)
  {
    _logger.LogInformation("GetShiftById");
    try
    {
      var redisKey = $"shift:{id}";

      var cachedShift = await _cacheService.Get<Shift>(redisKey);
      if (cachedShift != null)
      {
        return Ok(cachedShift);
      }

      var result = await _shiftRepo.GetByIdAsync(id);

      await _cacheService.Set(redisKey, result);
      return Ok(result);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [HttpPost]
  public async Task<IActionResult> Create(CreateShift createShift)
  {
    _logger.LogInformation("Create");
    try
    {
      var shift = new Shift
      {
        Id = Guid.NewGuid(),
        Start = createShift.Start,
        End = createShift.End,
        StudioId = createShift.StudioId
      };
      var result = await _shiftRepo.CreateAsync(shift);

      if (result > 0)
      {
        await _cacheService.ClearWithPattern("shifts");
        return Ok(result);
      }

      return ErrorResp.SomethingWrong("Something went wrong");
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }

  }

  [HttpPost("generate")]
  public async Task<IActionResult> GenerateShifts(GenerateShift generateShift)
  {
    _logger.LogInformation("GenerateShifts");
    try
    {
      var workingTimes = _studioRepo.GetStudioWorkingTime(generateShift.StudioId);

      var shifts = new List<Shift>();
      foreach (var workingTime in workingTimes)
      {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        var daysUntilClose = (int)workingTime.DayOfWeek - (int)tomorrow.DayOfWeek;
        if (daysUntilClose < 0)
        {
          daysUntilClose += 7;
        }
        var openAtDate = tomorrow.AddDays(daysUntilClose).Date + workingTime.OpenAt;
        var closeAtDate = tomorrow.AddDays(daysUntilClose).Date + workingTime.CloseAt;

        var shiftStart = openAtDate;

        while (shiftStart < closeAtDate)
        {
          var shiftEnd = shiftStart + generateShift.ShiftDuration;
          shifts.Add(new Shift
          {
            Id = Guid.NewGuid(),
            Start = shiftStart,
            End = shiftEnd,
            StudioId = generateShift.StudioId
          });
          shiftStart = shiftEnd;
        }
      }

      var result = await _shiftRepo.CreateAsync(shifts);

      if (result > 0)
      {
        await _cacheService.ClearWithPattern("shifts");
        return Ok(result);
      }

      return ErrorResp.SomethingWrong("Something went wrong");
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }

  }



  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(Guid id)
  {
    _logger.LogInformation("Delete");
    try
    {
      var result = await _shiftRepo.DeleteAsync(id);

      await _cacheService.Remove($"shift:{id}");
      await _cacheService.ClearWithPattern("shifts");
      return Ok(result);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }

  }
}
