using System.ComponentModel.DataAnnotations;
using art_tattoo_be.Application.Shared.Enum;

namespace art_tattoo_be.Application.DTOs.Invoice;

public class CreateInvoiceReq
{
  [Required]
  [Range(0, double.MaxValue, ErrorMessage = "Total must larger than 0")]
  public double Total { get; set; }
  [Required]
  public PayMethodEnum PayMethod { get; set; }
  public string? Notes { get; set; }
  public Guid? AppointmentId { get; set; }
  public Guid? ServiceId { get; set; }
  public Guid? UserId { get; set; }
  public bool IsGuest { get; set; }
}
