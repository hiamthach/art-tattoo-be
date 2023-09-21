namespace art_tattoo_be.Domain.RoleBase;

public class Permission
{
  public string Slug { get; set; } = null!;
  public string Name { get; set; } = null!;
  public string? Description { get; set; }

  public List<Role> Roles { get; } = new();
}