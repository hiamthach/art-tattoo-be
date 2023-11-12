using art_tattoo_be.Domain.Studio;

namespace art_tattoo_be.Domain.Invoice;

public class InvoiceService
{
  public Guid ServiceId { get; set; }
  public Guid InvoiceId { get; set; }
  public int Quantity { get; set; }
  public double Price { get; set; }
  public double Discount { get; set; }
  public virtual StudioService Service { get; set; } = null!;
  public virtual Invoice Invoice { get; set; } = null!;
}
