namespace art_tattoo_be.Application.DTOs.RoleBase;

using art_tattoo_be.Domain.RoleBase;
using AutoMapper;

public class PermissionDto
{
  public string Slug { get; set; } = null!;
  public string Name { get; set; } = null!;
  public string? Description { get; set; }
}

// AutoMapper
public class PermissionProfile : Profile
{
  public PermissionProfile()
  {
    CreateMap<Permission, PermissionDto>();
  }
}
