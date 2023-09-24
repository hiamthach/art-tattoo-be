namespace art_tattoo_be.Application.RoleBase;

public class CreatePermissionReq
{

  public string Name { get; set; } = null!;

  public string Slug { get; set; } = null!;

  public string? Description { get; set; }
}


public class CreatePermissionResp
{
  public string Message { get; set; } = null!;
}