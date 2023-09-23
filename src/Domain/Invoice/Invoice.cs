using art_tattoo_be.Application.Shared.Enum;

namespace art_tattoo_be.Domain.Invoice;

public class Invoice
{
  public Guid Id { get; set; }
  public Guid StudioId { get; set; }
  public Guid UserId { get; set; }
  public Guid AppointmentId { get; set; }
  public double Total { get; set; }
  public PayMethodEnum PayMethod { get; set; }
  public string? Notes { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}
