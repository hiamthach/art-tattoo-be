namespace art_tattoo_be.Application.DTOs.RoleBase;

using art_tattoo_be.Domain.RoleBase;
using AutoMapper;

public class RoleDto
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;

  public string? Description { get; set; }

  public List<PermissionDto> Permissions { get; set; } = new();
}

// AutoMapper
public class RoleProfile : Profile
{
  public RoleProfile()
  {
    CreateMap<Role, RoleDto>().ForMember(
      dest => dest.Permissions,
      opt => opt.MapFrom(src => src.Permissions)
    );
  }
}
