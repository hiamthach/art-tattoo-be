using art_tattoo_be.Application.DTOs.Shift;
using art_tattoo_be.Application.Middlewares;
using art_tattoo_be.Application.Shared;
using art_tattoo_be.Application.Shared.Handler;
using art_tattoo_be.Application.Shared.Helper;
using art_tattoo_be.Core.Jwt;
using art_tattoo_be.Domain.Booking;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Infrastructure.Cache;
using art_tattoo_be.Infrastructure.Database;
using art_tattoo_be.Infrastructure.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Cms;
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
      var redisKey = $"shifts:{query.Start.Ticks}:{query.End.Ticks}";

      if (query.ArtistId != null)
      {
        redisKey += $":a_{query.ArtistId}";
      }

      if (query.StudioId != null)
      {
        redisKey += $":stu_{query.StudioId}";
      }

      var cachedShifts = await _cacheService.Get<List<ShiftDto>>(redisKey);
      if (cachedShifts != null)
      {
        return Ok(cachedShifts);
      }

      var result = _shiftRepo.GetAllAsync(query);

      var mappedResult = _mapper.Map<List<ShiftDto>>(result);

      await _cacheService.Set(redisKey, mappedResult);
      return Ok(mappedResult);
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

      var cachedShift = await _cacheService.Get<ShiftDto>(redisKey);
      if (cachedShift != null)
      {
        return Ok(cachedShift);
      }

      var result = await _shiftRepo.GetByIdAsync(id);

      if (result == null)
      {
        return ErrorResp.NotFound("Shift not found");
      }

      var mappedResult = _mapper.Map<ShiftDto>(result);

      await _cacheService.Set(redisKey, mappedResult);
      return Ok(mappedResult);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_STUDIO_ARTIST_SCHEDULE)]
  [HttpPost]
  public async Task<IActionResult> Create(CreateShift createShift)
  {
    _logger.LogInformation("Create");
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
      var shift = new Shift
      {
        Id = Guid.NewGuid(),
        Start = createShift.Start,
        End = createShift.End,
        StudioId = studioId
      };
      var result = await _shiftRepo.CreateAsync(shift);

      if (result > 0)
      {
        await _cacheService.ClearWithPattern("shifts");
        return Ok(new BaseResp
        {
          Message = "Shift created successfully",
          Success = true
        });
      }

      return ErrorResp.SomethingWrong("Something went wrong");
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_STUDIO_ARTIST_SCHEDULE)]
  [HttpPost("generate")]
  public async Task<IActionResult> GenerateShifts(GenerateShift req)
  {
    _logger.LogInformation("GenerateShifts");
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
      var workingTimes = _studioRepo.GetStudioWorkingTime(studioId);
      if (workingTimes.Count() == 0)
      {
        return ErrorResp.BadRequest("Studio has no working time");
      }

      var shifts = new List<Shift>();
      var today = DateTime.Today;
      var tomorrow = today.AddDays(1);
      var weekNumber = DateHelper.GetWeekNumber(tomorrow);

      var redisWeekKey = $"shift-week:{studioId}:{weekNumber}";

      var checkWeekGenerated = await _cacheService.Get<bool>(redisWeekKey);
      if (checkWeekGenerated == true)
      {
        return ErrorResp.BadRequest("Shifts for this week already generated");
      }

      foreach (var workingTime in workingTimes)
      {
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
          var shiftEnd = shiftStart + req.ShiftDuration;
          shifts.Add(new Shift
          {
            Id = Guid.NewGuid(),
            Start = shiftStart,
            End = shiftEnd,
            StudioId = studioId
          });
          shiftStart = shiftEnd;
        }
      }

      var result = await _shiftRepo.CreateAsync(shifts);

      if (result > 0)
      {
        //   var weekNumber = DateHelper.GetWeekNumber();
        await _cacheService.Set(redisWeekKey, true, TimeSpan.FromDays(7));
        await _cacheService.ClearWithPattern("shifts");
        return Ok(new BaseResp
        {
          Message = "Shifts generated successfully",
          Success = true
        });
      }

      return ErrorResp.SomethingWrong("Something went wrong");
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }

  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_STUDIO_ARTIST_SCHEDULE)]
  [HttpPut("{id}")]
  public async Task<IActionResult> Update(Guid id, UpdateShift req)
  {
    _logger.LogInformation("Update");
    if (HttpContext.Items["payload"] is not Payload payload || HttpContext.Items["permission"] is not string permission)
    {
      return ErrorResp.Forbidden("You don't have permission to access this studio");
    }

    var studioId = _studioRepo.GetStudioIdByUserId(payload.UserId);

    try
    {
      var shift = await _shiftRepo.GetByIdAsync(id);
      if (shift == null)
      {
        return ErrorResp.NotFound("Shift not found");
      }

      if (shift.StudioId != studioId)
      {
        return ErrorResp.Forbidden("You don't have permission to access this studio");
      }

      shift.Start = req.Start;
      shift.End = req.End;

      var result = await _shiftRepo.UpdateAsync(id, shift);

      if (result > 0)
      {
        await _cacheService.Remove($"shift:{id}");
        await _cacheService.ClearWithPattern("shifts");
        return Ok(new BaseResp
        {
          Message = "Shift updated successfully",
          Success = true
        });
      }

      return ErrorResp.SomethingWrong("Something went wrong");
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_STUDIO_ARTIST_SCHEDULE)]
  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(Guid id)
  {
    _logger.LogInformation("Delete");
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
      var shift = await _shiftRepo.GetByIdAsync(id);
      if (shift == null)
      {
        return ErrorResp.NotFound("Shift not found");
      }

      if (shift.StudioId != studioId)
      {
        return ErrorResp.Forbidden("You don't have permission to access this studio");
      }

      var result = await _shiftRepo.DeleteAsync(id);

      await _cacheService.Remove($"shift:{id}");
      await _cacheService.ClearWithPattern("shifts");
      return Ok(new BaseResp
      {
        Message = "Shift deleted successfully",
        Success = true
      });
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }

  }
}
