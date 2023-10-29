namespace art_tattoo_be.Application.DTOs.Shift;

using AutoMapper;
using art_tattoo_be.Domain.Booking;

public class ShiftDto
{
  public Guid Id { get; set; }
  public DateTime Start { get; set; }
  public DateTime End { get; set; }
  public Guid StudioId { get; set; }
  public List<ShiftUserDto> ShiftUsers { get; set; } = new();
}

public class ShiftUserDto
{
  public Guid ShiftId { get; set; }
  public Guid StuUserId { get; set; }
  public bool IsBooked { get; set; }
}

public class ShiftProfile : Profile
{
  public ShiftProfile()
  {
    CreateMap<Shift, ShiftDto>()
      .ForMember(dest => dest.ShiftUsers, opt => opt.MapFrom(src => src.ShiftUsers))
      .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
  }
}

public class ShiftUserProfile : Profile
{
  public ShiftUserProfile()
  {
    CreateMap<ShiftUser, ShiftUserDto>();
  }
}
