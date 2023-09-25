namespace art_tattoo_be.Domain.RoleBase;

public class Permission
{
  public string Slug { get; set; } = null!;
  public string Name { get; set; } = null!;
  public string? Description { get; set; }

  public List<Role> Roles { get; } = new();

  public PermissionDto ToDto()
  {
    return new PermissionDto
    {
      Slug = Slug,
      Name = Name,
      Description = Description
    };
  }
}

public class PermissionDto
{
  public string Slug { get; set; } = null!;
  public string Name { get; set; } = null!;
  public string? Description { get; set; }
}
