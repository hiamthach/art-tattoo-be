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

[ApiController]
[Route("api/appointment")]
public class AppointmentController : ControllerBase
{
  private readonly ILogger<AppointmentController> _logger;
  private readonly IAppointmentRepository _appointmentRepo;
  private readonly IShiftRepository _shiftRepo;
  private readonly IMapper _mapper;
  private readonly ICacheService _cacheService;

  public AppointmentController(
    ILogger<AppointmentController> logger,
    ArtTattooDbContext dbContext,
    IMapper mapper,
    ICacheService cacheService
  )
  {
    _logger = logger;
    _appointmentRepo = new AppointmentRepository(dbContext);
    _shiftRepo = new ShiftRepository(dbContext);
    _mapper = mapper;
    _cacheService = cacheService;
  }


  [Protected]
  [HttpPost]
  public async Task<IActionResult> BookAppointment(AppointmentCreate body)
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
        if (shiftUser != null)
        {
          shiftUser.IsBooked = true;
          await _shiftRepo.UpdateShiftUserAsync(shiftUser);
        }

        if (bookCount == 0)
        {
          await _cacheService.Set($"bookCount:{payload.UserId}", 1, TimeSpan.FromHours(12));
        }
        else
        {
          await _cacheService.Update($"bookCount:{payload.UserId}", bookCount + 1);
        }

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

}
