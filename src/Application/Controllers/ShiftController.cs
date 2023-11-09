namespace art_tattoo_be.Application.Controllers;

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
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

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
  public async Task<IActionResult> GetShifts([FromQuery] ShiftQueryReq query)
  {
    _logger.LogInformation("GetShifts");
    if (DateTime.Compare(query.End, query.Start) <= 0)
    {
      return ErrorResp.BadRequest("End date must be greater than start date");
    }

    try
    {
      var redisKey = $"shifts:{query.Start.Ticks}:{query.End.Ticks}:stu_{query.StudioId}";

      if (query.ArtistId != null)
      {
        redisKey += $":art_{query.ArtistId}";
      }

      var cachedShifts = await _cacheService.Get<List<ShiftDto>>(redisKey);
      if (cachedShifts != null)
      {
        return Ok(cachedShifts);
      }

      var q = new ShiftQuery()
      {
        Start = query.Start,
        End = query.End,
        StudioId = query.StudioId,
        ArtistId = query.ArtistId,
        IsStudio = false
      };

      var result = _shiftRepo.GetAllAsync(q);

      var mappedResult = _mapper.Map<List<ShiftDto>>(result);

      await _cacheService.Set(redisKey, mappedResult);
      return Ok(mappedResult);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [HttpGet("studio")]
  [Permission(PermissionSlugConst.MANAGE_STUDIO_ARTIST_SCHEDULE, PermissionSlugConst.VIEW_STUDIO_ARTIST_SCHEDULE)]
  public async Task<IActionResult> GetStudioShifts([FromQuery] ShiftQueryReq query)
  {
    _logger.LogInformation("GetShifts");
    if (DateTime.Compare(query.End, query.Start) <= 0)
    {
      return ErrorResp.BadRequest("End date must be greater than start date");
    }

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
      var redisKey = $"shifts:stu_{studioId}:{query.Start.Ticks}:{query.End.Ticks}:stu_{query.StudioId}";

      if (query.ArtistId != null)
      {
        redisKey += $":art_{query.ArtistId}";
      }

      var cachedShifts = await _cacheService.Get<List<ShiftDto>>(redisKey);
      if (cachedShifts != null)
      {
        return Ok(cachedShifts);
      }

      var q = new ShiftQuery()
      {
        Start = query.Start,
        End = query.End,
        StudioId = query.StudioId,
        ArtistId = query.ArtistId,
        IsStudio = true
      };

      var result = _shiftRepo.GetAllAsync(q);

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
  [Permission(PermissionSlugConst.VIEW_STUDIO_ARTIST_SCHEDULE)]
  [HttpGet("artist")]
  public async Task<IActionResult> GetArtistShifts([FromQuery] ShiftQueryDate query)
  {
    _logger.LogInformation("GetArtistShifts");
    if (HttpContext.Items["payload"] is not Payload payload || HttpContext.Items["permission"] is not string permission)
    {
      return ErrorResp.Forbidden("You don't have permission to access this studio");
    }

    var studioId = _studioRepo.GetStudioIdByUserId(payload.UserId);
    var studioUserId = _studioRepo.GetStudioUserIdByUserId(payload.UserId);
    if (studioId == Guid.Empty)
    {
      return ErrorResp.Forbidden("You don't have permission to access this studio");
    }

    try
    {
      var redisKey = $"shifts:{query.Start.Ticks}:{query.End.Ticks}:stu_{studioId}";

      redisKey += $":art_{studioUserId}";

      var q = new ShiftQuery()
      {
        Start = query.Start,
        End = query.End,
        StudioId = studioId,
        ArtistId = studioUserId
      };

      var cachedShifts = await _cacheService.Get<List<ShiftDto>>(redisKey);
      if (cachedShifts != null)
      {
        return Ok(cachedShifts);
      }

      var result = _shiftRepo.GetAllAsync(q);

      var mappedResult = _mapper.Map<List<ShiftDto>>(result);

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
      if (!workingTimes.Any())
      {
        return ErrorResp.BadRequest("Studio has no working time");
      }

      var shifts = new List<Shift>();
      var today = DateTime.Today;

      var redisKey = $"shift-generate:stu_{studioId}";

      var cachedEnd = await _cacheService.Get<DateTime>(redisKey);

      if (cachedEnd != default(DateTime))
      {
        today = cachedEnd;
      }

      // check the request end date is greater than today and less than 14 days
      if (DateTime.Compare(req.End, today) <= 0)
      {
        return ErrorResp.BadRequest("End date must be greater than today");
      }

      if (DateTime.Compare(req.End, today.AddDays(14)) > 0)
      {
        return ErrorResp.BadRequest("End date must be less than 14 days");
      }

      // generate shifts from tomorrow to end date
      var tomorrow = today.AddDays(1);
      while (tomorrow <= req.End)
      {
        // dayOfWeek: 0 - 6
        int dayOfWeek = (int)tomorrow.DayOfWeek;
        var workingTime = workingTimes.FirstOrDefault(w => w.DayOfWeek == dayOfWeek);
        if (workingTime == null)
        {
          tomorrow = tomorrow.AddDays(1);
          continue;
        }

        var openAtDate = tomorrow.Date + workingTime.OpenAt;
        var closeAtDate = tomorrow.Date + workingTime.CloseAt;

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

        tomorrow = tomorrow.AddDays(1);
        continue;
      }

      var result = await _shiftRepo.CreateAsync(shifts);

      if (result > 0)
      {
        //   var weekNumber = DateHelper.GetWeekNumber();
        // await _cacheService.Set(redisWeekKey, true, TimeSpan.FromDays(7));
        await _cacheService.Set(redisKey, req.End, TimeSpan.FromDays(14));
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

      if (DateTime.Compare(req.End, req.Start) <= 0)
      {
        return ErrorResp.BadRequest("End date must be greater than start date");
      }

      shift.Start = req.Start;
      shift.End = req.End;

      var result = await _shiftRepo.UpdateAsync(id, shift, req);

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

  [Protected]
  [Permission(PermissionSlugConst.VIEW_STUDIO_ARTIST_SCHEDULE)]
  [HttpPut("register/{id}")]
  public async Task<IActionResult> RegisterShift(Guid id)
  {
    _logger.LogInformation("RegisterShift");
    if (HttpContext.Items["payload"] is not Payload payload || HttpContext.Items["permission"] is not string permission)
    {
      return ErrorResp.Forbidden("You don't have permission to access this studio");
    }

    var studioId = _studioRepo.GetStudioIdByUserId(payload.UserId);
    var studioUserId = _studioRepo.GetStudioUserIdByUserId(payload.UserId);
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

      var result = await _shiftRepo.RegisterUserAsync(id, studioUserId);

      if (result > 0)
      {
        await _cacheService.Remove($"shift:{id}");
        await _cacheService.ClearWithPattern("shifts");
        return Ok(new BaseResp
        {
          Message = "Shift registered successfully",
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
}
