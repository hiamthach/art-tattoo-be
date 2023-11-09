namespace art_tattoo_be.Application.DTOs.Appointment;

public class AppointmentCreate
{
  public Guid ShiftId { get; set; }
  public string? Notes { get; set; }
  public Guid? ArtistId { get; set; }
  public Guid? ServiceId { get; set; }
}
