namespace art_tattoo_be.Domain.Studio;

public class Studio
{
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;
  public string? Detail { get; set; }
  public string? Logo { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}