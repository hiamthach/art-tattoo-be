namespace art_tattoo_be.Infrastructure.Repository;

using art_tattoo_be.Application.DTOs.Analytics;
using art_tattoo_be.Application.DTOs.Invoice;
using art_tattoo_be.Domain.Invoice;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

public class InvoiceRepository : IInvoiceRepository
{
  private readonly ArtTattooDbContext _dbContext;

  public InvoiceRepository(ArtTattooDbContext context)
  {
    _dbContext = context;
  }

  public InvoiceList GetAllAsync(InvoiceQuery query)
  {
    var q = _dbContext.Invoices
      .Include(i => i.User)
      .Include(i => i.Studio)
      .Include(i => i.Appointment).ThenInclude(a => a.Shift)
      // .Include(i => i.InvoiceServices).ThenInclude(i => i.Service)
      .Where(app =>
        (query.SearchKeyword == null || app.User.FullName.Contains(query.SearchKeyword) || app.User.Email.Contains(query.SearchKeyword) || (app.User.Phone != null && app.User.Phone.Contains(query.SearchKeyword))) &&
        (query.StudioId == null || app.StudioId == query.StudioId) &&
        (query.UserId == null || app.UserId == query.UserId) &&
        (query.ServiceList == null || app.InvoiceServices.Any(i => query.ServiceList.Contains(i.ServiceId.ToString())))
      )
      .Select(i => new Invoice
      {
        Id = i.Id,
        UserId = i.UserId,
        StudioId = i.StudioId,
        AppointmentId = i.AppointmentId,
        CreatedAt = i.CreatedAt,
        UpdatedAt = i.UpdatedAt,
        User = i.User,
        Total = i.Total,
        PayMethod = i.PayMethod,
        Notes = i.Notes,
        Studio = new Studio
        {
          Id = i.Studio.Id,
          Name = i.Studio.Name,
          Address = i.Studio.Address,
          Phone = i.Studio.Phone,
          Email = i.Studio.Email,
          Logo = i.Studio.Logo,
        },
        Appointment = i.Appointment,
      })
      ;

    var totalCount = q.Count();

    var pagedResults = q
      .OrderByDescending(i => i.CreatedAt)
      .Skip(query.PageSize * query.Page)
      .Take(query.PageSize)
      .ToList();

    return new InvoiceList
    {
      Invoices = pagedResults,
      Total = totalCount
    };
  }

  public Invoice? GetByIdAsync(Guid id)
  {
    return _dbContext.Invoices
      .Include(i => i.User)
      .Include(i => i.Studio)
      .Include(i => i.Appointment).ThenInclude(a => a.Shift)
      .Include(i => i.InvoiceServices).ThenInclude(i => i.Service)
      .Select(i => new Invoice
      {
        Id = i.Id,
        UserId = i.UserId,
        StudioId = i.StudioId,
        AppointmentId = i.AppointmentId,
        CreatedAt = i.CreatedAt,
        UpdatedAt = i.UpdatedAt,
        User = i.User,
        Total = i.Total,
        PayMethod = i.PayMethod,
        Notes = i.Notes,
        Studio = new Studio
        {
          Id = i.Studio.Id,
          Name = i.Studio.Name,
          Address = i.Studio.Address,
          Phone = i.Studio.Phone,
          Email = i.Studio.Email,
          Logo = i.Studio.Logo,
          Rating = i.Studio.Rating,
        },
        Appointment = i.Appointment,
        InvoiceServices = i.InvoiceServices
      })
      .FirstOrDefault(i => i.Id == id);
  }

  public async Task<int> CreateAsync(Invoice invoice)
  {
    await _dbContext.Invoices.AddAsync(invoice);
    return await _dbContext.SaveChangesAsync();
  }

  public async Task<int> UpdateAsync(Invoice invoice)
  {
    _dbContext.Invoices.Update(invoice);
    return await _dbContext.SaveChangesAsync();
  }

  public RevenueStudioDashboard GetRevenueStudioDashboard(Guid studioId)
  {
    var q = _dbContext.Invoices
      .Where(i => i.StudioId == studioId)
      .Select(i => new
      {
        i.CreatedAt,
        i.Total
      });

    var totalRevenue = q.Sum(i => i.Total);
    var totalRevenueThisMonth = q.Where(i => i.CreatedAt.Month == DateTime.Now.Month).Sum(i => i.Total);
    var totalRevenueLastMonth = q.Where(i => i.CreatedAt.Month == DateTime.Now.AddMonths(-1).Month).Sum(i => i.Total);

    return new RevenueStudioDashboard
    {
      TotalRevenue = totalRevenue,
      TotalRevenueThisMonth = totalRevenueThisMonth,
      TotalRevenueLastMonth = totalRevenueLastMonth
    };
  }
}
