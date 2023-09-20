namespace art_tattoo_be.Domain.RoleBase;

using AutoMapper;

public class Role
{
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;

  public ICollection<RolePermission> RolePermission { get; set; } = null!;
}

public class RoleDto
{
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;
  public IEnumerable<Permission> Permissions { get; set; } = new List<Permission>();
}

public class RoleProfile : Profile
{
  public RoleProfile()
  {
    CreateMap<Role, RoleDto>()
      .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.RolePermission.Select(rp => rp.Permission).ToList()));
  }
}