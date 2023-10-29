namespace art_tattoo_be.Application.DTOs.Appointment;

using art_tattoo_be.Application.DTOs.Media;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Booking;
using AutoMapper;

public class AppointmentDto
{
  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public Guid ShiftId { get; set; }
  public Guid? DoneBy { get; set; }
  public string? Notes { get; set; }
  public AppointmentStatusEnum Status { get; set; }
  public List<MediaDto> ListMedia { get; set; } = new();
}

public class AppointmentProfile : Profile
{
  public AppointmentProfile()
  {
    CreateMap<Appointment, AppointmentDto>();
  }
}
