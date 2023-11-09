namespace art_tattoo_be.Application.DTOs.Appointment;

using art_tattoo_be.Application.DTOs.Media;
using art_tattoo_be.Application.DTOs.Pagination;
using art_tattoo_be.Application.DTOs.Shift;
using art_tattoo_be.Application.DTOs.Studio;
using art_tattoo_be.Application.DTOs.User;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Booking;
using art_tattoo_be.src.Application.DTOs.StudioService;
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
  public StudioUserDto? Artist { get; set; }
  public UserDto User { get; set; } = null!;
  public ShiftDto Shift { get; set; } = null!;
  public StudioServiceDto? Service { get; set; }
}

public class AppointmentResp : PaginationResp
{
  public List<AppointmentDto> Appointments { get; set; } = new List<AppointmentDto>();
}

public class AppointmentProfile : Profile
{
  public AppointmentProfile()
  {
    CreateMap<Appointment, AppointmentDto>()
      .ForMember(dest => dest.ListMedia, opt => opt.MapFrom(src => src.ListMedia))
      .ForMember(dest => dest.Shift, opt => opt.MapFrom(src => src.Shift))
      .ForMember(dest => dest.Artist, opt => opt.MapFrom(src => src.Artist))
      .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
      .ForMember(dest => dest.Service, opt => opt.MapFrom(src => src.Service));
  }
}
