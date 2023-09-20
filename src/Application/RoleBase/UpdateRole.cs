namespace art_tattoo_be.Application.RoleBase;

public class UpdateRoleReq
{

  public IEnumerable<Guid> PermissionIds { get; set; } = new List<Guid>();
}