namespace art_tattoo_be.Domain.Studio;

using art_tattoo_be.Domain.Booking;
using art_tattoo_be.Domain.Category;
using art_tattoo_be.Domain.Media;
using art_tattoo_be.Domain.Invoice;

public class StudioService
{
  public Guid Id { get; set; }
  public Guid StudioId { get; set; }
  public int CategoryId { get; set; }
  public string Name { get; set; } = null!;
  public string Description { get; set; } = null!;
  public double MinPrice { get; set; }
  public double MaxPrice { get; set; }
  public double Discount { get; set; }
  public bool IsDisabled { get; set; }
  public string Thumbnail { get; set; } = null!;
  public TimeSpan? ExpectDuration { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  public Studio Studio { get; set; } = null!;
  public Category Category { get; set; } = null!;
  public List<Media> ListMedia { get; set; } = new();
  public List<Appointment> Appointments { get; set; } = new();
  public List<InvoiceService> InvoiceServices { get; set; } = new();
}
public class StudioServiceList
{
  public IEnumerable<StudioService> StudioServices { get; set; } = new List<StudioService>();
  public int TotalCount { get; set; }
}
