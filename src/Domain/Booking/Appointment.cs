namespace art_tattoo_be.Domain.Booking;

public class Appointment
{
  public Guid Id { get; set; }
  public Guid StudioId { get; set; }
  public Guid UserId { get; set; }
  public Guid ScheduleId { get; set; }
  public Guid DoneBy { get; set; }
  public string? Notes { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}
