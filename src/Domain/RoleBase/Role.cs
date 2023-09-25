namespace art_tattoo_be.Domain.RoleBase;
using art_tattoo_be.Domain.User;

public class Role
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;

  public string? Description { get; set; }

  public List<Permission> Permissions { get; } = new();
  public List<User> Users { get; } = new();

  public RoleDto ToDto()
  {
    return new RoleDto
    {
      Id = Id,
      Name = Name,
      Description = Description,
      Permissions = Permissions.Select(p => p.ToDto()).ToList()
    };
  }
}

public class RoleDto
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;

  public string? Description { get; set; }

  public List<PermissionDto> Permissions { get; set; } = new();
}
