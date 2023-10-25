namespace art_tattoo_be.Domain.Booking;

using art_tattoo_be.Domain.Studio;

public class Shift
{
  public Guid Id { get; set; }
  public Guid StudioId { get; set; }
  public DateTime Start { get; set; }
  public DateTime End { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  public Studio Studio { get; set; } = null!;
  public List<StudioUser> Artists { get; set; } = null!;
  public List<Appointment> Appointments { get; set; } = null!;
}
