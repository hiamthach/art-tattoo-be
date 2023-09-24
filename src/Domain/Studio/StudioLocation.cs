namespace art_tattoo_be.Domain.Studio;

public class StudioLocation
{
  public Guid Id { get; set; }
  public Guid StudioId { get; set; }
  public string Address { get; set; } = null!;
  public string? Province { get; set; }
  public string? City { get; set; }
  public string? Country { get; set; }
  public string? PostalCode { get; set; }
  public string? Latitude { get; set; }
  public string? Longitude { get; set; }

  public virtual Studio Studio { get; set; } = null!;
}
