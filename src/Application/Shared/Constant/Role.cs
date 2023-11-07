namespace art_tattoo_be.Application.Shared.Constant;

public class RoleConst
{
  public const string ADMIN = "ADMIN";
  public const int ADMIN_ID = 1;
  public const string SYSTEM_STAFF = "SYSTEM_STAFF";
  public const int SYSTEM_STAFF_ID = 2;
  public const string STUDIO_MANAGER = "STUDIO_MANAGER";
  public const int STUDIO_MANAGER_ID = 3;
  public const string STUDIO_STAFF = "STUDIO_STAFF";
  public const int STUDIO_STAFF_ID = 4;
  public const string ARTIST = "ARTIST";
  public const int ARTIST_ID = 5;
  public const string MEMBER = "MEMBER";
  public const int MEMBER_ID = 6;
  public Dictionary<string, int> RoleId = new()
  {
    { ADMIN, ADMIN_ID },
    { SYSTEM_STAFF, SYSTEM_STAFF_ID },
    { STUDIO_MANAGER, STUDIO_MANAGER_ID },
    { STUDIO_STAFF, STUDIO_STAFF_ID },
    { ARTIST, ARTIST_ID },
    { MEMBER, MEMBER_ID }
  };

  public static int GetRoleId(string roleName)
  {
    return new RoleConst().RoleId[roleName];
  }
}
