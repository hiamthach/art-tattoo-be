namespace art_tattoo_be.Domain.Invoice;

using art_tattoo_be.Application.Shared.Enum;
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
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  public virtual Studio Studio { get; set; } = null!;
  public virtual User User { get; set; } = null!;
}
