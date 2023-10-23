namespace art_tattoo_be.Application.DTOs.User;

using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.User;
using AutoMapper;

public class UserDto
{
  public Guid Id { get; set; }
  public string Email { get; set; } = null!;
  public string FullName { get; set; } = null!;
  public string? Phone { get; set; }
  public string? Address { get; set; }
  public string? Avatar { get; set; }
  public DateTime? Birthday { get; set; }
  public int RoleId { get; set; }
  public UserStatusEnum Status { get; set; }
}

public class UserProfile : Profile
{
  public UserProfile()
  {
    CreateMap<UserDto, User>();
    CreateMap<User, UserDto>();
    CreateMap<UpdateUserReq, User>()
      .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    CreateMap<UpdateUserProfileReq, User>()
      .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    CreateMap<CreateUserReq, User>();
  }
}
