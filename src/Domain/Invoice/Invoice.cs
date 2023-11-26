namespace art_tattoo_be.Domain.Invoice;

using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Booking;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Domain.User;

public class Invoice
{
  public Guid Id { get; set; }
  public Guid StudioId { get; set; }
  public Guid UserId { get; set; }
  public double Total { get; set; }
  public PayMethodEnum PayMethod { get; set; }
  public string? Notes { get; set; }
  public Guid? AppointmentId { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  public virtual Studio Studio { get; set; } = null!;
  public virtual User User { get; set; } = null!;
  public virtual List<InvoiceService> InvoiceServices { get; set; } = new();
  public virtual Appointment Appointment { get; set; } = null!;
}

public class InvoiceList
{
  public List<Invoice> Invoices { get; set; } = null!;
  public int Total { get; set; }
}
