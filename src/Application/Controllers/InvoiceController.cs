namespace art_tattoo_be.Application.Controllers;

using art_tattoo_be.Application.DTOs.Invoice;
using art_tattoo_be.Application.Middlewares;
using art_tattoo_be.Application.Shared;
using art_tattoo_be.Application.Shared.Constant;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Application.Shared.Handler;
using art_tattoo_be.Core.Jwt;
using art_tattoo_be.Domain.Booking;
using art_tattoo_be.Domain.Invoice;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Domain.User;
using art_tattoo_be.Infrastructure.Cache;
using art_tattoo_be.Infrastructure.Database;
using art_tattoo_be.Infrastructure.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/invoice")]
public class InvoiceController : ControllerBase
{
  private readonly ILogger<InvoiceController> _logger;
  private readonly IMapper _mapper;
  private readonly ICacheService _cacheService;
  private readonly IInvoiceRepository _invoiceRepository;
  private readonly IStudioRepository _studioRepo;
  private readonly IAppointmentRepository _appointmentRepo;
  private readonly IUserRepository _userRepo;

  public InvoiceController(ILogger<InvoiceController> logger, IMapper mapper, ICacheService cacheService, ArtTattooDbContext dbContext)
  {
    _logger = logger;
    _mapper = mapper;
    _cacheService = cacheService;
    _invoiceRepository = new InvoiceRepository(dbContext);
    _studioRepo = new StudioRepository(dbContext);
    _appointmentRepo = new AppointmentRepository(dbContext);
    _userRepo = new UserRepository(dbContext);
  }

  [HttpGet("pay-method")]
  public async Task<IActionResult> GetInvoicePayMethod()
  {
    _logger.LogInformation("GetInvoicePayMethod");

    try
    {
      var redisKey = "pay-method";
      var cached = await _cacheService.Get<Dictionary<int, string>>(redisKey);
      if (cached != null)
      {
        return Ok(cached);
      }

      var statuses = Enum.GetValues<PayMethodEnum>();

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
  public async Task<IActionResult> GetOwnInvoice([FromQuery] GetUserInvoiceQuery query)
  {
    _logger.LogInformation("GetInvoices");

    if (HttpContext.Items["payload"] is not Payload payload)
    {
      return ErrorResp.Unauthorized("Unauthorized");
    }

    try
    {
      var redisKey = $"invoices:own:{payload.UserId}:{query.Page}:{query.PageSize}";

      if (query.SearchKeyword != null)
      {
        redisKey += $"?search={query.SearchKeyword}";
      }

      var cached = await _cacheService.Get<InvoiceResp>(redisKey);
      if (cached != null)
      {
        return Ok(cached);
      }

      var q = new InvoiceQuery
      {
        UserId = payload.UserId,
        Page = query.Page,
        PageSize = query.PageSize,
        SearchKeyword = query.SearchKeyword
      };

      var invoices = _invoiceRepository.GetAllAsync(q);

      var resp = new InvoiceResp
      {
        Invoices = _mapper.Map<List<InvoiceDto>>(invoices.Invoices),
        Total = invoices.Total,
        Page = query.Page,
        PageSize = query.PageSize
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
  [Permission(PermissionSlugConst.MANAGE_STUDIO_INVOICE, PermissionSlugConst.VIEW_STUDIO_INVOICE)]
  [HttpGet("studio")]
  public async Task<IActionResult> GetStudioInvoice([FromQuery] GetUserInvoiceQuery query)
  {
    _logger.LogInformation("GetStudioAppointments");

    if (HttpContext.Items["payload"] is not Payload payload)
    {
      return ErrorResp.Unauthorized("Unauthorized");
    }

    var studioId = _studioRepo.GetStudioIdByUserId(payload.UserId);

    try
    {
      var redisKey = $"invoices:stu_{studioId}:{query.Page}:{query.PageSize}";

      if (query.SearchKeyword != null)
      {
        redisKey += $"?search={query.SearchKeyword}";
      }

      var cached = await _cacheService.Get<InvoiceResp>(redisKey);
      if (cached != null)
      {
        return Ok(cached);
      }

      var q = new InvoiceQuery
      {
        StudioId = studioId,
        Page = query.Page,
        PageSize = query.PageSize,
        SearchKeyword = query.SearchKeyword
      };

      var invoices = _invoiceRepository.GetAllAsync(q);

      var resp = new InvoiceResp
      {
        Invoices = _mapper.Map<List<InvoiceDto>>(invoices.Invoices),
        Total = invoices.Total,
        Page = query.Page,
        PageSize = query.PageSize
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
  public async Task<IActionResult> GetInvoiceDetail(Guid id)
  {
    _logger.LogInformation("GetInvoice");

    if (HttpContext.Items["payload"] is not Payload payload)
    {
      return ErrorResp.Unauthorized("Unauthorized");
    }

    try
    {
      var redisKey = $"invoice:own:{payload.UserId}:{id}";

      var cached = await _cacheService.Get<InvoiceDto>(redisKey);
      if (cached != null)
      {
        if (cached.UserId != payload.UserId)
        {
          return ErrorResp.Forbidden("You don't have permission to view this invoice");
        }
        return Ok(cached);
      }

      var invoice = _invoiceRepository.GetByIdAsync(id);

      if (invoice == null)
      {
        return ErrorResp.NotFound("Invoice not found");
      }

      if (invoice.UserId != payload.UserId)
      {
        return ErrorResp.Forbidden("You don't have permission to view this invoice");
      }

      var resp = _mapper.Map<InvoiceDto>(invoice);

      await _cacheService.Set(redisKey, resp);

      return Ok(resp);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [HttpGet("studio/{id}")]
  public async Task<IActionResult> GetStudioInvoiceDetail(Guid id)
  {
    _logger.LogInformation("GetStudioAppointments");

    if (HttpContext.Items["payload"] is not Payload payload)
    {
      return ErrorResp.Unauthorized("Unauthorized");
    }

    var studioId = _studioRepo.GetStudioIdByUserId(payload.UserId);

    try
    {
      var redisKey = $"invoice:stu_{studioId}:{id}";

      var cached = await _cacheService.Get<InvoiceDto>(redisKey);
      if (cached != null)
      {
        if (cached.StudioId != studioId)
        {
          return ErrorResp.Forbidden("You don't have permission to view this invoice");
        }
        return Ok(cached);
      }

      var invoice = _invoiceRepository.GetByIdAsync(id);

      if (invoice == null)
      {
        return ErrorResp.NotFound("Invoice not found");
      }

      if (invoice.StudioId != studioId)
      {
        return ErrorResp.Forbidden("You don't have permission to view this invoice");
      }

      var resp = _mapper.Map<InvoiceDto>(invoice);

      await _cacheService.Set(redisKey, resp);

      return Ok(resp);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_STUDIO_INVOICE)]
  [HttpPost]
  public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceReq req)
  {
    _logger.LogInformation("CreateInvoice");

    if (HttpContext.Items["payload"] is not Payload payload)
    {
      return ErrorResp.Unauthorized("Unauthorized");
    }

    var studioId = _studioRepo.GetStudioIdByUserId(payload.UserId);

    try
    {
      Appointment? appointment = null;
      if (req.AppointmentId != null)
      {
        appointment = _appointmentRepo.GetByIdAsync(req.AppointmentId.Value);
        if (appointment == null)
        {
          return ErrorResp.NotFound("Appointment not found");
        }

        if (appointment.Shift.StudioId != studioId)
        {
          return ErrorResp.Forbidden("You don't have permission to create invoice for this appointment");
        }

        if (appointment.Status != AppointmentStatusEnum.Confirmed && appointment.Status != AppointmentStatusEnum.Reschedule)
        {
          return ErrorResp.Forbidden("You can only create invoice for confirmed appointment");
        }
      }

      if (req.UserId != null)
      {
        var user = _userRepo.GetUserById(req.UserId.Value);
        if (user == null)
        {
          return ErrorResp.NotFound("User not found");
        }
      }

      var invoice = new Invoice
      {
        Id = Guid.NewGuid(),
        StudioId = studioId,
        UserId = req.UserId != null ? req.UserId.Value : Guid.Parse(UserConst.USER_GUEST),
        Total = req.Total,
        PayMethod = req.PayMethod,
        Notes = req.Notes,
        AppointmentId = req.AppointmentId,
      };

      if (req.IsGuest)
      {
        invoice.UserId = Guid.Parse(UserConst.USER_GUEST);
        invoice.AppointmentId = null;
      }

      var result = await _invoiceRepository.CreateAsync(invoice);

      if (result == 0)
      {
        return ErrorResp.SomethingWrong("Create invoice failed");
      }

      if (req.AppointmentId != null && appointment != null)
      {
        appointment.Status = AppointmentStatusEnum.Completed;
        await _appointmentRepo.UpdateAsync(appointment);
      }

      // await _cacheService.ClearWithPattern($"invoices:own:${payload.UserId}");
      // await _cacheService.ClearWithPattern($"invoices:stu_${studioId}");
      await _cacheService.ClearWithPattern($"invoices");
      await _cacheService.ClearWithPattern($"appointments");

      return Ok(new BaseResp
      {
        Message = "Create invoice successfully",
        Success = true,

      });
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }
}
