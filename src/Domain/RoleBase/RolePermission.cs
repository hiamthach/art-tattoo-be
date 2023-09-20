namespace art_tattoo_be.Domain.RoleBase;

public class RolePermission
{
  public Guid RoleId { get; set; }
  public Guid PermissionId { get; set; }

  public Permission? Permission { get; set; }
  public Role? Role { get; set; }
}