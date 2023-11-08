namespace art_tattoo_be.Application.Controllers;

using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using art_tattoo_be.Infrastructure.Cache;
using art_tattoo_be.Domain.Booking;
using art_tattoo_be.Infrastructure.Database;
using art_tattoo_be.Infrastructure.Repository;
using art_tattoo_be.Application.Middlewares;
using art_tattoo_be.Application.DTOs.Appointment;
using art_tattoo_be.Application.Shared.Handler;
using art_tattoo_be.Core.Jwt;
using art_tattoo_be.Application.Shared;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Application.Shared.Constant;
using art_tattoo_be.Domain.RoleBase;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Application.DTOs.Shift;
using art_tattoo_be.Core.Mail;

[ApiController]
[Route("api/appointment")]
public class AppointmentController : ControllerBase
{
  private readonly ILogger<AppointmentController> _logger;
  private readonly IAppointmentRepository _appointmentRepo;
  private readonly IShiftRepository _shiftRepo;
  private readonly IStudioRepository _studioRepo;
  private readonly IMapper _mapper;
  private readonly ICacheService _cacheService;
  private readonly IMailService _mailService;


  public AppointmentController(
    ILogger<AppointmentController> logger,
    ArtTattooDbContext dbContext,
    IMapper mapper,
    IMailService mailService,
    ICacheService cacheService
  )
  {
    _logger = logger;
    _appointmentRepo = new AppointmentRepository(dbContext);
    _shiftRepo = new ShiftRepository(dbContext);
    _studioRepo = new StudioRepository(dbContext);
    _mapper = mapper;
    _cacheService = cacheService;
    _mailService = mailService;
  }

  [HttpGet("status")]
  public async Task<IActionResult> GetAppointmentStatus()
  {
    _logger.LogInformation("GetAppointmentStatus");

    try
    {
      var redisKey = "appointment-status";
      var cached = await _cacheService.Get<Dictionary<int, string>>(redisKey);
      if (cached != null)
      {
        return Ok(cached);
      }

      var statuses = Enum.GetValues<AppointmentStatusEnum>();

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

  [Protected]
  [HttpGet]
  public async Task<IActionResult> GetAppointments([FromQuery] GetAppointmentsQuery query)
  {
    _logger.LogInformation("GetAppointments");

    if (HttpContext.Items["payload"] is not Payload payload)
    {
      return ErrorResp.Unauthorized("Unauthorized");
    }

    try
    {
      var redisKey = $"appointments:{payload.UserId}:{query.Page}:{query.PageSize}";

      if (query.StudioId != null)
      {
        redisKey += $":{query.StudioId}";
      }

      if (query.StatusList != null)
      {
        var statusString = query.StatusList.Aggregate("", (current, status) => current + $"{status},");
        redisKey += $"?status={statusString}";
      }

      if (query.StartDate != null)
      {
        redisKey += $"?startDate={query.StartDate?.Ticks}";
      }

      if (query.EndDate != null)
      {
        redisKey += $"?endDate={query.EndDate?.Ticks}";
      }

      var cached = await _cacheService.Get<AppointmentResp>(redisKey);
      if (cached != null)
      {
        return Ok(cached);
      }

      var appointmentQuery = new AppointmentQuery
      {
        Page = query.Page,
        PageSize = query.PageSize,
        StatusList = query.StatusList,
        StudioId = query.StudioId == Guid.Empty ? null : query.StudioId,
        UserId = payload.UserId,
        StartDate = query.StartDate,
        EndDate = query.EndDate,
      };

      var appointments = _appointmentRepo.GetAllAsync(appointmentQuery);
      var mapped = _mapper.Map<List<AppointmentDto>>(appointments.Appointments);

      var resp = new AppointmentResp
      {
        Appointments = mapped,
        Page = query.Page,
        PageSize = query.PageSize,
        Total = appointments.TotalCount,
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
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAppointmentDetail(Guid id)
  {
    _logger.LogInformation("GetAppointmentDetail");

    if (HttpContext.Items["payload"] is not Payload payload)
    {
      return ErrorResp.Unauthorized("Unauthorized");
    }

    try
    {
      var redisKey = $"appointment:{id}";

      var cached = await _cacheService.Get<AppointmentDto>(redisKey);
      if (cached != null)
      {
        if (cached.UserId != payload.UserId)
        {
          return ErrorResp.Unauthorized("Unauthorized");
        }

        return Ok(cached);
      }

      var appointment = _appointmentRepo.GetByIdAsync(id);

      if (appointment == null)
      {
        return ErrorResp.NotFound("Appointment not found");
      }

      if (appointment.UserId != payload.UserId)
      {
        return ErrorResp.Unauthorized("Unauthorized");
      }

      var mapped = _mapper.Map<AppointmentDto>(appointment);
      await _cacheService.Set(redisKey, mapped);

      return Ok(mapped);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [HttpPost]
  public async Task<IActionResult> BookAppointment([FromBody] AppointmentCreate body)
  {
    _logger.LogInformation("BookAppointment");

    if (HttpContext.Items["payload"] is not Payload payload)
    {
      return ErrorResp.Unauthorized("Unauthorized");
    }
    try
    {
      var bookCount = await _cacheService.Get<int>($"bookCount:{payload.UserId}");
      if (bookCount >= BookConst.MaxBookCount)
      {
        return ErrorResp.BadRequest($"You can only book {BookConst.MaxBookCount} appointments per 12 hours");
      }
      ShiftUser? shiftUser = null;
      if (body.ArtistId != null)
      {
        var su = await _shiftRepo.GetShiftUserAsync(body.ShiftId, (Guid)body.ArtistId);

        if (su == null)
        {
          return ErrorResp.NotFound("Shift not found");
        }
        else
        {
          shiftUser = su;
        }
      }

      if (_appointmentRepo.IsBooked(body.ShiftId, payload.UserId))
      {
        return ErrorResp.BadRequest("You have already booked appointment at this time");
      }

      var appointment = new Appointment
      {
        Id = Guid.NewGuid(),
        ShiftId = body.ShiftId,
        Notes = body.Notes,
        DoneBy = body.ArtistId,
        UserId = payload.UserId,
        Status = AppointmentStatusEnum.Pending,
      };

      var result = await _appointmentRepo.CreateAsync(appointment);

      if (result > 0)
      {
        if (bookCount == 0)
        {
          await _cacheService.Set($"bookCount:{payload.UserId}", 1, TimeSpan.FromHours(12));
        }
        else
        {
          await _cacheService.Update($"bookCount:{payload.UserId}", bookCount + 1);
        }

        await _cacheService.ClearWithPattern($"appointments");

        return Ok(new BaseResp
        {
          Message = "Appointment booked successfully",
          Success = true,
        });
      }
      else
      {
        return ErrorResp.SomethingWrong("Something went wrong");
      }
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [HttpPut("reschedule/{id}")]
  public async Task<IActionResult> RescheduleAppointment(Guid id, [FromBody] AppointmentCreate body)
  {
    _logger.LogInformation("RescheduleAppointment");

    if (HttpContext.Items["payload"] is not Payload payload)
    {
      return ErrorResp.Unauthorized("Unauthorized");
    }

    try
    {
      var appointment = _appointmentRepo.GetByIdAsync(id);

      if (appointment == null)
      {
        return ErrorResp.NotFound("Appointment not found");
      }

      if (appointment.UserId != payload.UserId)
      {
        return ErrorResp.Unauthorized("Unauthorized");
      }

      switch (appointment.Status)
      {
        case AppointmentStatusEnum.Canceled:
          return ErrorResp.BadRequest("Appointment already canceled");
        case AppointmentStatusEnum.Completed:
          return ErrorResp.BadRequest("Appointment already Completed");
        case AppointmentStatusEnum.Late:
          return ErrorResp.BadRequest("Appointment already Late");
      }

      if (appointment.Status == AppointmentStatusEnum.Pending || appointment.Status == AppointmentStatusEnum.Confirmed || appointment.Status == AppointmentStatusEnum.Reschedule)
      {
        appointment.ShiftId = body.ShiftId;
        appointment.Notes = body.Notes ?? appointment.Notes;
        appointment.DoneBy = body.ArtistId ?? appointment.DoneBy;
        appointment.Status = AppointmentStatusEnum.Reschedule;

        var result = await _appointmentRepo.UpdateAsync(appointment);

        if (result > 0)
        {
          await _cacheService.ClearWithPattern($"appointments");
          await _cacheService.Remove($"appointment:{id}");
          return Ok(new BaseResp
          {
            Message = "Appointment rescheduled successfully",
            Success = true,
          });
        }
        else
        {
          return ErrorResp.SomethingWrong("Something went wrong");
        }
      }
      else
      {
        return ErrorResp.BadRequest("Appointment cannot be rescheduled");
      }
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [HttpPut("cancel/{id}")]
  public async Task<IActionResult> CancelAppointment(Guid id)
  {
    _logger.LogInformation("CancelAppointment");

    if (HttpContext.Items["payload"] is not Payload payload)
    {
      return ErrorResp.Unauthorized("Unauthorized");
    }

    try
    {
      var appointment = _appointmentRepo.GetByIdAsync(id);

      if (appointment == null)
      {
        return ErrorResp.NotFound("Appointment not found");
      }

      if (appointment.UserId != payload.UserId)
      {
        return ErrorResp.Unauthorized("Unauthorized");
      }

      switch (appointment.Status)
      {
        case AppointmentStatusEnum.Canceled:
          return ErrorResp.BadRequest("Appointment already canceled");
        case AppointmentStatusEnum.Completed:
          return ErrorResp.BadRequest("Appointment already Completed");
        case AppointmentStatusEnum.Late:
          return ErrorResp.BadRequest("Appointment already Late");
      }

      if (appointment.Status == AppointmentStatusEnum.Pending || appointment.Status == AppointmentStatusEnum.Confirmed || appointment.Status == AppointmentStatusEnum.Reschedule)
      {
        if (appointment.Shift.ShiftUsers != null && appointment.DoneBy != null)
        {
          var shiftUser = appointment.Shift.ShiftUsers.FirstOrDefault(su => su.StuUserId == appointment.DoneBy);
          if (shiftUser != null)
          {
            shiftUser.IsBooked = false;
            await _shiftRepo.UpdateShiftUserAsync(shiftUser);
          }

          if (appointment.Duration != null)
          {
            var startTime = appointment.Shift.Start;
            var endTime = startTime.AddHours(appointment.Duration.Value.TotalHours);

            var shiftsOfUser = _shiftRepo.GetAllAsync(new ShiftQuery
            {
              ArtistId = appointment.DoneBy,
              Start = startTime,
              End = endTime,
              IsStudio = true,
              StudioId = appointment.Shift.StudioId,
            });

            if (shiftsOfUser != null)
            {
              for (int i = 0; i < shiftsOfUser.Count(); i++)
              {
                var su = shiftsOfUser.ElementAt(i).ShiftUsers.FirstOrDefault(su => su.StuUserId == appointment.DoneBy);
                if (su != null)
                {
                  su.IsBooked = false;
                  await _shiftRepo.UpdateShiftUserAsync(su);
                }
              }
            }
            else
            {
              return ErrorResp.BadRequest("Artist is not available at this time");
            }
          }
        }

        var result = await _appointmentRepo.UpdateStatusAsync(id, AppointmentStatusEnum.Canceled);

        if (result > 0)
        {
          await _cacheService.ClearWithPattern($"appointments");
          await _cacheService.ClearWithPattern($"shifts");
          await _cacheService.Remove($"appointment:{id}");
          return Ok(new BaseResp
          {
            Message = "Appointment canceled successfully",
            Success = true,
          });
        }
        else
        {
          return ErrorResp.SomethingWrong("Something went wrong");
        }
      }
      else
      {
        return ErrorResp.BadRequest("Appointment cannot be canceled");
      }
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }


  // studio api
  [Protected]
  [Permission(PermissionSlugConst.MANAGE_STUDIO_BOOKING, PermissionSlugConst.VIEW_STUDIO_BOOKING)]
  [HttpGet("studio")]
  public async Task<IActionResult> GetStudioAppointments([FromQuery] GetStudioAppointmentsQuery query)
  {
    _logger.LogInformation("GetStudioAppointments");

    if (HttpContext.Items["payload"] is not Payload payload)
    {
      return ErrorResp.Unauthorized("Unauthorized");
    }

    var studioId = _studioRepo.GetStudioIdByUserId(payload.UserId);

    try
    {
      var redisKey = $"appointments:studio_{studioId}:{query.Page}:{query.PageSize}";

      if (query.StartDate != null)
      {
        redisKey += $":{query.StartDate?.Ticks}";
      }

      if (query.EndDate != null)
      {
        redisKey += $":{query.EndDate?.Ticks}";
      }

      if (query.StatusList != null)
      {
        var statusString = query.StatusList.Aggregate("", (current, status) => current + $"{status},");
        redisKey += $"?status={statusString}";
      }

      if (query.SearchKeyword != null)
      {
        redisKey += $"?search={query.SearchKeyword}";
      }

      var cached = await _cacheService.Get<AppointmentResp>(redisKey);
      if (cached != null)
      {
        return Ok(cached);
      }

      var appointmentQuery = new AppointmentQuery
      {
        Page = query.Page,
        PageSize = query.PageSize,
        StudioId = studioId,
        UserId = null,
        StartDate = query.StartDate,
        EndDate = query.EndDate,
        StatusList = query.StatusList,
        SearchKeyword = query.SearchKeyword,
      };

      var appointments = _appointmentRepo.GetAllAsync(appointmentQuery);
      var mapped = _mapper.Map<List<AppointmentDto>>(appointments.Appointments);

      var resp = new AppointmentResp
      {
        Appointments = mapped,
        Page = query.Page,
        PageSize = query.PageSize,
        Total = appointments.TotalCount,
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
  [Permission(PermissionSlugConst.MANAGE_STUDIO_BOOKING, PermissionSlugConst.VIEW_STUDIO_BOOKING)]
  [HttpGet("studio/{id}")]
  public async Task<IActionResult> GetStudioAppointmentDetail(Guid id)
  {
    _logger.LogInformation("GetStudioAppointmentDetail");

    if (HttpContext.Items["payload"] is not Payload payload)
    {
      return ErrorResp.Unauthorized("Unauthorized");
    }

    var studioId = _studioRepo.GetStudioIdByUserId(payload.UserId);

    try
    {
      var redisKey = $"appointments:studio_{studioId}:{id}";

      var cached = await _cacheService.Get<AppointmentDto>(redisKey);
      if (cached != null)
      {
        if (cached.Shift.StudioId != studioId)
        {
          return ErrorResp.Unauthorized("You are not allowed to view this appointment");
        }
        return Ok(cached);
      }

      var appointment = _appointmentRepo.GetByIdAsync(id);

      if (appointment == null)
      {
        return ErrorResp.NotFound("Appointment not found");
      }

      if (appointment.Shift.StudioId != studioId)
      {
        return ErrorResp.Unauthorized("You are not allowed to view this appointment");
      }

      var mapped = _mapper.Map<AppointmentDto>(appointment);
      await _cacheService.Set(redisKey, mapped);

      return Ok(mapped);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_STUDIO_BOOKING)]
  [HttpPut("studio/{id}")]
  public async Task<IActionResult> UpdateStudioAppointment(Guid id, [FromBody] AppointmentUpdate body)
  {
    _logger.LogInformation("UpdateStudioAppointment");

    if (HttpContext.Items["payload"] is not Payload payload)
    {
      return ErrorResp.Unauthorized("Unauthorized");
    }

    var studioId = _studioRepo.GetStudioIdByUserId(payload.UserId);

    try
    {
      var appointment = _appointmentRepo.GetByIdAsync(id);

      if (appointment == null)
      {
        return ErrorResp.NotFound("Appointment not found");
      }

      if (appointment.Shift.StudioId != studioId)
      {
        return ErrorResp.Unauthorized("You are not allowed to update this appointment");
      }

      switch (appointment.Status)
      {
        case AppointmentStatusEnum.Canceled:
          return ErrorResp.BadRequest("Appointment already canceled");
        case AppointmentStatusEnum.Completed:
          return ErrorResp.BadRequest("Appointment already Completed");
        case AppointmentStatusEnum.Late:
          return ErrorResp.BadRequest("Appointment already Late");
      }

      if (appointment.Status == AppointmentStatusEnum.Pending || appointment.Status == AppointmentStatusEnum.Confirmed || appointment.Status == AppointmentStatusEnum.Reschedule)
      {
        appointment.ShiftId = body.ShiftId ?? appointment.ShiftId;
        appointment.Notes = body.Notes ?? appointment.Notes;
        appointment.DoneBy = body.ArtistId ?? appointment.DoneBy;
        appointment.Status = body.Status ?? appointment.Status;
        appointment.Duration = body.Duration ?? appointment.Duration;

        // Confirmation or Reschedule appointment
        if (body.Status != AppointmentStatusEnum.Pending && appointment.DoneBy != null)
        {
          if (body.Duration != null)
          {
            var startTime = appointment.Shift.Start;
            var endTime = startTime.AddHours(body.Duration.Value.TotalHours);

            var shiftsOfUser = _shiftRepo.GetAllAsync(new ShiftQuery
            {
              ArtistId = appointment.DoneBy,
              Start = startTime,
              End = endTime,
              IsStudio = true,
              StudioId = appointment.Shift.StudioId,
            });

            if (shiftsOfUser != null)
            {
              if (shiftsOfUser.Any(s => s.ShiftUsers.Any(su => su.IsBooked)))
              {
                return ErrorResp.BadRequest("Artist is not available at this time");
              }
              else
              {
                for (int i = 0; i < shiftsOfUser.Count(); i++)
                {
                  var shiftUser = shiftsOfUser.ElementAt(i).ShiftUsers.FirstOrDefault(su => su.StuUserId == appointment.DoneBy);
                  if (shiftUser != null)
                  {
                    shiftUser.IsBooked = true;
                    await _shiftRepo.UpdateShiftUserAsync(shiftUser);
                  }
                }
              }
            }
            else
            {
              return ErrorResp.BadRequest("Artist is not available at this time");
            }
          }

          if (appointment.Shift.ShiftUsers != null)
          {
            var shiftUser = appointment.Shift.ShiftUsers.FirstOrDefault(su => su.StuUserId == appointment.DoneBy);
            if (shiftUser != null && shiftUser.IsBooked == false)
            {
              shiftUser.IsBooked = true;
              await _shiftRepo.UpdateShiftUserAsync(shiftUser);
            }
          }
        }

        if (body.Status == AppointmentStatusEnum.Canceled || body.Status == AppointmentStatusEnum.Reschedule)
        {
          if (appointment.Shift.ShiftUsers != null && appointment.DoneBy != null)
          {
            var shiftUser = appointment.Shift.ShiftUsers.FirstOrDefault(su => su.StuUserId == appointment.DoneBy);
            if (shiftUser != null)
            {
              shiftUser.IsBooked = false;
              await _shiftRepo.UpdateShiftUserAsync(shiftUser);
            }

            if (appointment.Duration != null)
            {
              var startTime = appointment.Shift.Start;
              var endTime = startTime.AddHours(appointment.Duration.Value.TotalHours);

              var shiftsOfUser = _shiftRepo.GetAllAsync(new ShiftQuery
              {
                ArtistId = appointment.DoneBy,
                Start = startTime,
                End = endTime,
                IsStudio = true,
                StudioId = appointment.Shift.StudioId,
              });

              if (shiftsOfUser != null)
              {
                for (int i = 0; i < shiftsOfUser.Count(); i++)
                {
                  var su = shiftsOfUser.ElementAt(i).ShiftUsers.FirstOrDefault(su => su.StuUserId == appointment.DoneBy);
                  if (shiftUser != null)
                  {
                    shiftUser.IsBooked = false;
                    await _shiftRepo.UpdateShiftUserAsync(shiftUser);
                  }
                }

              }
              else
              {
                return ErrorResp.BadRequest("Artist is not available at this time");
              }
            }
          }
        }

        var result = await _appointmentRepo.UpdateAsync(appointment);

        if (result > 0)
        {
          await _cacheService.ClearWithPattern($"appointments");
          await _cacheService.ClearWithPattern($"shifts");
          await _cacheService.Remove($"appointment:{id}");
          if (_mailService != null && appointment.User != null)
          {
            await _mailService.SendEmailAsync(appointment.User.Email, "Lịch hẹn thay đổi", $"Lịch hẹn của bạn ở {appointment.Shift.Studio.Name} vừa cập nhật. Vui lòng kiểm tra lại lịch hẹn của bạn.");
          }
          return Ok(new BaseResp
          {
            Message = "Appointment updated successfully",
            Success = true,
          });
        }
        else
        {
          return ErrorResp.SomethingWrong("Something went wrong");
        }
      }
      else
      {
        return ErrorResp.BadRequest("Appointment cannot be updated");
      }
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }
}
