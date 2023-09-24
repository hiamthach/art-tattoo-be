namespace art_tattoo_be.Application.RoleBase;

public class UpdateRoleReq
{

  public IEnumerable<string> PermissionIds { get; set; } = new List<string>();
}