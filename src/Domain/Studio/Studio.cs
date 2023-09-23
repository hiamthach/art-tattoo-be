namespace art_tattoo_be.Domain.Studio;

using art_tattoo_be.Domain.Testimonial;

public class Studio
{
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;
  public string? Detail { get; set; }
  public string? Logo { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
  public List<StudioService> Services { get; set; } = new();
  public List<StudioWorkingTime> WorkingTimes { get; set; } = new();
  public List<StudioUser> StudioUsers { get; set; } = new();
  public List<Testimonial> Testimonials { get; set; } = new();
}
