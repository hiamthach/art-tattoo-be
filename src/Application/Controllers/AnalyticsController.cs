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
  [Permission(PermissionSlugConst.MANAGE_STUDIO, PermissionSlugConst.MANAGE_USERS, PermissionSlugConst.MANAGE_TESTIMONIAL)]
  [HttpGet("admin-dashboard/booking-daily")]
  public IActionResult GetAdminDashboardBookingDaily()
  {
    _logger.LogInformation("GetAdminDashboardBookingDaily");
    try
    {
      var bookingDaily = _appointmentRepo.GetBookingDaily();

      return Ok(bookingDaily);
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
}
