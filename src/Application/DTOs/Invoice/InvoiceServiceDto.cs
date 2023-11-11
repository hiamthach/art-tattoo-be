namespace art_tattoo_be.Application.DTOs.Invoice;

using art_tattoo_be.src.Application.DTOs.StudioService;

public class InvoiceServiceDto
{
  public int Quantity { get; set; }
  public double Price { get; set; }
  public double Discount { get; set; }
  public virtual StudioServiceDto Service { get; set; } = null!;
}
