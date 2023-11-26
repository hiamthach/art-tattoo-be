using art_tattoo_be.Application.DTOs.Analytics;
using art_tattoo_be.Application.DTOs.Invoice;

namespace art_tattoo_be.Domain.Invoice;

public interface IInvoiceRepository
{
  RevenueStudioDashboard GetRevenueStudioDashboard(Guid studioId);
  InvoiceList GetAllAsync(InvoiceQuery query);
  Invoice? GetByIdAsync(Guid id);
  Task<int> CreateAsync(Invoice invoice);
  Task<int> UpdateAsync(Invoice invoice);
}
