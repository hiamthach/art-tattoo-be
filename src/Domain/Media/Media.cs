namespace art_tattoo_be.Domain.Media;

using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Domain.User;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Booking;

public class Media
{
  public Guid Id { get; set; }
  public string Url { get; set; } = null!;
  public MediaTypeEnum Type { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  public List<Studio> StudioMedia { get; set; } = null!;
  public List<User> UserMedia { get; set; } = null!;
  public List<StudioService> StudioServiceMedia { get; set; } = null!;
  public List<Appointment> AppointmentMedia { get; set; } = null!;
}
