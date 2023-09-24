namespace art_tattoo_be.Domain.Booking;

using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Domain.User;
using art_tattoo_be.Domain.Media;

public class Appointment
{
  public Guid Id { get; set; }
  public Guid StudioId { get; set; }
  public Guid UserId { get; set; }
  public Guid ScheduleId { get; set; }
  public Guid? DoneBy { get; set; }
  public string? Notes { get; set; }
  public AppointmentStatusEnum Status { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  public virtual Studio Studio { get; set; } = null!;
  public virtual User User { get; set; } = null!;
  public virtual Schedule Schedule { get; set; } = null!;
  public virtual StudioUser Artist { get; set; } = null!;
  public virtual List<Media> ListMedia { get; set; } = new();
}
