namespace art_tattoo_be.Domain.Studio;

using art_tattoo_be.Domain.Testimonial;
using art_tattoo_be.Domain.Booking;
using art_tattoo_be.Domain.Invoice;
using art_tattoo_be.Domain.Media;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Blog;

public class Studio
{
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;
  public string? Slogan { get; set; }
  public string? Introduction { get; set; }
  public string? Detail { get; set; }
  public string? Logo { get; set; }
  public string Phone { get; set; } = null!;
  public string Email { get; set; } = null!;
  public string? Website { get; set; }
  public string? Facebook { get; set; }
  public string? Instagram { get; set; }
  public string Address { get; set; } = null!;
  public double Latitude { get; set; }
  public double Longitude { get; set; }
  public StudioStatusEnum Status { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  public List<StudioService> Services { get; set; } = new();
  public List<StudioWorkingTime> WorkingTimes { get; set; } = new();
  public List<StudioUser> StudioUsers { get; set; } = new();
  public List<Testimonial> Testimonials { get; set; } = new();
  public List<Appointment> Appointments { get; set; } = new();
  public List<Invoice> Invoices { get; set; } = new();
  public List<Media> ListMedia { get; set; } = new();
  public List<Blog> Blogs { get; set; } = new();
  public List<Shift> Shifts { get; set; } = new();
}

public class StudioList
{
  public IEnumerable<Studio> Studios { get; set; } = new List<Studio>();
  public int TotalCount { get; set; }
}
