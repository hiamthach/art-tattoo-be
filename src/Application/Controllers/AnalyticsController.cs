namespace art_tattoo_be.Application.Controllers;

using art_tattoo_be.Application.DTOs.Analytics;
using art_tattoo_be.Application.Middlewares;
using art_tattoo_be.Application.Shared;
using art_tattoo_be.Domain.Booking;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Domain.Testimonial;
using art_tattoo_be.Domain.User;
using art_tattoo_be.Infrastructure.Cache;
using art_tattoo_be.Infrastructure.Database;
using art_tattoo_be.Infrastructure.Repository;
using art_tattoo_be.src.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using art_tattoo_be.Application.Shared.Handler;
using art_tattoo_be.Domain.Invoice;

[Produces("application/json")]
[ApiController]
[Route("api/analytics")]
public class AnalyticsController : ControllerBase
{
  private readonly ILogger<AnalyticsController> _logger;
  private readonly IMapper _mapper;
  private readonly IStudioRepository _studioRepository;
  private readonly ICacheService _cacheService;
  private readonly IUserRepository _userRepo;
  private readonly ITestimonialRepository _testimonialRepo;
  private readonly IAppointmentRepository _appointmentRepo;
  private readonly IInvoiceRepository _invoiceRepo;

  public AnalyticsController(
    ILogger<AnalyticsController> logger,
    IMapper mapper,
    ICacheService cacheService,
    ArtTattooDbContext dbContext
  )
  {
    _logger = logger;
    _mapper = mapper;
    _cacheService = cacheService;
    _studioRepository = new StudioRepository(dbContext);
    _userRepo = new UserRepository(dbContext);
    _testimonialRepo = new TestimonialRepository(dbContext);
    _appointmentRepo = new AppointmentRepository(dbContext);
    _invoiceRepo = new InvoiceRepository(dbContext);
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_STUDIO, PermissionSlugConst.MANAGE_USERS, PermissionSlugConst.MANAGE_TESTIMONIAL)]
  [HttpGet("admin-dashboard")]
  public IActionResult GetAdminDashboard()
  {
    _logger.LogInformation("GetAdminDashboard");
    try
    {
      var studioData = _studioRepository.GetStudioAdminDashboard();
      var userData = _userRepo.GetUserAdminDashboard();
      var testimonialData = _testimonialRepo.GetTestimonialAdminDashboard();
      var bookingData = _appointmentRepo.GetBookingAdminDashboard();

      var adminDashboard = new AdminDashboard
      {
        StudioData = studioData,
        UserData = userData,
        TestimonialData = testimonialData,
        BookingData = bookingData
      };

      return Ok(adminDashboard);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }

  }

  [Protected]
  [HttpGet("studio-dashboard/{id}")]
  public async Task<IActionResult> GetStudioDashboard([FromRoute] Guid id)
  {
    _logger.LogInformation("GetStudioDashboard");
    try
    {
      var studio = await _studioRepository.GetAsync(id);
      if (studio == null)
      {
        return ErrorResp.NotFound("Studio not found");
      }

      var userData = _appointmentRepo.GetUserBookingDashboard(id);
      var testimonialData = _testimonialRepo.GetTestimonialStudioDashboard(id);
      var bookingData = _appointmentRepo.GetBookingStudioDashboard(id);
      var revenueData = _invoiceRepo.GetRevenueStudioDashboard(id);

      var adminDashboard = new StudioDashboard
      {
        UserData = userData,
        TestimonialData = testimonialData,
        BookingData = bookingData,
        RevenueData = revenueData
      };

      return Ok(adminDashboard);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }


  [Protected]
  [Permission(PermissionSlugConst.MANAGE_STUDIO, PermissionSlugConst.MANAGE_USERS, PermissionSlugConst.MANAGE_TESTIMONIAL)]
  [HttpGet("admin-dashboard/booking-daily")]
  public async Task<IActionResult> GetAdminDashboardBookingDaily([FromQuery] Guid studioId)
  {
    _logger.LogInformation("GetAdminDashboardBookingDaily");
    try
    {
      if (studioId != Guid.Empty)
      {
        var studio = await _studioRepository.GetAsync(studioId);
        if (studio == null)
        {
          return ErrorResp.NotFound("Studio not found");
        }
        var bookingStudioDaily = _appointmentRepo.GetBookingDaily(studioId);

        return Ok(bookingStudioDaily);
      }

      var bookingDaily = _appointmentRepo.GetBookingDaily();

      return Ok(bookingDaily);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [HttpGet("studio-dashboard/booking-daily/{id}")]
  public async Task<IActionResult> GetStudioDashboardBookingDaily([FromRoute] Guid id)
  {
    _logger.LogInformation("GetAdminDashboardBookingDaily");
    try
    {
      var studio = await _studioRepository.GetAsync(id);
      if (studio == null)
      {
        return ErrorResp.NotFound("Studio not found");
      }
      var bookingStudioDaily = _appointmentRepo.GetBookingDaily(id);

      return Ok(bookingStudioDaily);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_STUDIO, PermissionSlugConst.MANAGE_USERS, PermissionSlugConst.MANAGE_TESTIMONIAL)]
  [HttpGet("admin-dashboard/most-popular-studio")]
  public IActionResult GetAdminDashboardPopularStudio()
  {
    _logger.LogInformation("GetAdminDashboardPopularStudio");
    try
    {
      var mostPopularStudio = _appointmentRepo.GetMostPopularStudio();

      return Ok(mostPopularStudio);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [HttpGet("studio-dashboard/most-popular-artist/{id}")]
  public async Task<IActionResult> GetStudioDashboardPopularArtist([FromRoute] Guid id)
  {
    _logger.LogInformation("GetStudioDashboardPopularArtist");
    try
    {
      var studio = await _studioRepository.GetAsync(id);
      if (studio == null)
      {
        return ErrorResp.NotFound("Studio not found");
      }
      var mostPopularArtist = _appointmentRepo.GetMostPopularArtist(id);

      return Ok(mostPopularArtist);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }
}
