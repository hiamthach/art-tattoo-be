namespace art_tattoo_be.Domain.RoleBase;

public class Role
{
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;

  public ICollection<RolePermission> RolePermission { get; set; } = null!;
}