namespace art_tattoo_be.Application.DTOs.Appointment;

using art_tattoo_be.Application.Shared.Enum;

public class AppointmentUpdate
{
  public Guid? ShiftId { get; set; }
  public string? Notes { get; set; }
  public Guid? ArtistId { get; set; }
  public Guid? ServiceId { get; set; }
  public AppointmentStatusEnum? Status { get; set; }
  public TimeSpan? Duration { get; set; }
}
