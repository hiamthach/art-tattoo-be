namespace art_tattoo_be.Application.RoleBase;

public class UpdateRoleReq
{

  public string Name { get; set; } = null!;

  public string Slug { get; set; } = null!;

  public string? Description { get; set; }
}