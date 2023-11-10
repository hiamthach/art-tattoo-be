namespace art_tattoo_be.Infrastructure.Repository;

using art_tattoo_be.Application.DTOs.Invoice;
using art_tattoo_be.Domain.Invoice;
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
      .Include(i => i.Service)
      .Where(app =>
        (query.SearchKeyword == null || app.User.FullName.Contains(query.SearchKeyword) || app.User.Email.Contains(query.SearchKeyword) || (app.User.Phone != null && app.User.Phone.Contains(query.SearchKeyword))) &&
        (query.StudioId == null || app.StudioId == query.StudioId) &&
        (query.UserId == null || app.UserId == query.UserId) &&
        (query.ServiceList == null || app.ServiceId != null && query.ServiceList.Contains(app.ServiceId.ToString()))
      );

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
      .Include(i => i.Service)
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
}
