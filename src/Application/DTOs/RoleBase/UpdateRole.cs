namespace art_tattoo_be.Application.DTOs.RoleBase;

public class UpdateRoleReq
{

  public IEnumerable<string> PermissionIds { get; set; } = new List<string>();
}
