namespace art_tattoo_be.Domain.RoleBase;

using AutoMapper;

public class Role
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;

  public string? Description { get; set; }

  public List<Permission> Permissions { get; } = new();
}