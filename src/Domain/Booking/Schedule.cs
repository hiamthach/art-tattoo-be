namespace art_tattoo_be.Domain.Booking;

using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Domain.User;

public class Schedule
{
  public Guid Id { get; set; }
  public Guid ArtistId { get; set; }
  public DateTime Start { get; set; }
  public DateTime End { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  public virtual StudioUser Artist { get; set; } = null!;
  public List<Appointment> Appointments { get; set; } = null!;
}
