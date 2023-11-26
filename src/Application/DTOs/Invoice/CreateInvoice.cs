using System.ComponentModel.DataAnnotations;
using art_tattoo_be.Application.Shared.Enum;

namespace art_tattoo_be.Application.DTOs.Invoice;

public class CreateInvoiceReq
{
  [Required]
  public PayMethodEnum PayMethod { get; set; }
  public string? Notes { get; set; }
  public Guid? AppointmentId { get; set; }
  public Guid? UserId { get; set; }
  public bool IsGuest { get; set; }
  public List<CreateInvoiceService> Services { get; set; } = new();
}

public class CreateInvoiceService
{
  public Guid ServiceId { get; set; }
  public int Quantity { get; set; }
  public double Price { get; set; }
  public double Discount { get; set; }
}
