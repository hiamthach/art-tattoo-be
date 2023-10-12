namespace art_tattoo_be.Domain.Studio;

using art_tattoo_be.Domain.User;
using art_tattoo_be.Domain.Booking;

public class StudioUser
{
  public Guid Id { get; set; }
  public Guid StudioId { get; set; }
  public Guid UserId { get; set; }
  public bool? IsDisabled { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  public Studio Studio { get; set; } = null!;
  public User User { get; set; } = null!;

  public List<Shift> Shifts { get; set; } = new();
  public List<Appointment> Appointments { get; set; } = new();
}
