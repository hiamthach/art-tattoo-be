namespace art_tattoo_be.Domain.Studio;

using art_tattoo_be.Domain.Testimonial;
using art_tattoo_be.Domain.Booking;
using art_tattoo_be.Domain.Invoice;
using art_tattoo_be.Domain.Media;

public class Studio
{
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;
  public string? Detail { get; set; }
  public string? Logo { get; set; }
  public string? Phone { get; set; }
  public string? Email { get; set; }
  public string? Website { get; set; }
  public string? Facebook { get; set; }
  public string? Instagram { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  public List<StudioLocation> Locations { get; set; } = new();
  public List<StudioService> Services { get; set; } = new();
  public List<StudioWorkingTime> WorkingTimes { get; set; } = new();
  public List<StudioUser> StudioUsers { get; set; } = new();
  public List<Testimonial> Testimonials { get; set; } = new();
  public List<Appointment> Appointments { get; set; } = new();
  public List<Invoice> Invoices { get; set; } = new();
  public List<Media> ListMedia { get; set; } = new();
}
