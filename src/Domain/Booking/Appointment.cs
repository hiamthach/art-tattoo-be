namespace art_tattoo_be.Domain.Booking;

using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Domain.User;
using art_tattoo_be.Domain.Media;
using art_tattoo_be.Domain.Invoice;

public class Appointment
{
  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public Guid ShiftId { get; set; }
  public Guid? DoneBy { get; set; }
  public string? Notes { get; set; }
  public AppointmentStatusEnum Status { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  public virtual User User { get; set; } = null!;
  public virtual Shift Shift { get; set; } = null!;
  public virtual StudioUser Artist { get; set; } = null!;
  public virtual List<Invoice> ListInvoice { get; set; } = new();
  public virtual List<Media> ListMedia { get; set; } = new();
}

public class AppointmentList
{
  public IEnumerable<Appointment> Appointments { get; set; } = new List<Appointment>();
  public int TotalCount { get; set; }
}
