namespace art_tattoo_be.Application.Shared.Constant;

public class RoleConst
{
  public const string ADMIN = "ADMIN";
  public const string SYSTEM_STAFF = "SYSTEM_STAFF";
  public const string STUDIO_MANAGER = "STUDIO_MANAGER";
  public const string STUDIO_STAFF = "STUDIO_STAFF";
  public const string ARTIST = "ARTIST";

  public const string MEMBER = "MEMBER";

  public Dictionary<string, int> RoleId = new()
  {
    { ADMIN, 1 },
    { SYSTEM_STAFF, 2 },
    { STUDIO_MANAGER, 3 },
    { STUDIO_STAFF, 4 },
    { ARTIST, 5 },
    { MEMBER, 6 }
  };

  public static int GetRoleId(string roleName)
  {
    return new RoleConst().RoleId[roleName];
  }
}
