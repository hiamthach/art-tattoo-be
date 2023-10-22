namespace art_tattoo_be.Application.DTOs.Studio;

using AutoMapper;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Application.DTOs.User;

public class StudioUserDto
{
  public Guid Id { get; set; }
  public Guid StudioId { get; set; }
  public Guid UserId { get; set; }
  public bool? IsDisabled { get; set; }
  public UserDto User { get; set; } = null!;
}

public class StudioUserDtoProfile : Profile
{
  public StudioUserDtoProfile()
  {
    CreateMap<StudioUser, StudioUserDto>().ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
  }
}
