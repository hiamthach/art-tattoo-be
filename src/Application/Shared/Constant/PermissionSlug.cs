namespace art_tattoo_be.Application.Shared;

public static class PermissionSlugConst
{
  public const string MANAGE_USERS = "USR.ALL";
  public const string MANAGE_ROLE = "ROLE.ALL";
  public const string MANAGE_PERMISSION = "PER.ALL";
  public const string MANAGE_CATEGORY = "CATE.ALL";
  public const string MANAGE_BLOG = "BLOG.ALL";
  public const string MANAGE_OWNED_BLOG = "BLOG.OWN";
  // studio permission
  public const string MANAGE_STUDIO = "STU.ALL";
  public const string MANAGE_OWNED_STUDIO = "STU.OWN";
  public const string MANAGE_STUDIO_ARTIST_SCHEDULE = "STU_AS.ALL";
  public const string MANAGE_STUDIO_ARTISTS = "STU_A.ALL";
  public const string MANAGE_STUDIO_SERVICES = "STU_S.ALL";
  public const string MANAGE_STUDIO_BOOKING = "STU_B.ALL";
  public const string MANAGE_STUDIO_INVOICE = "STU_I.ALL";
  public const string VIEW_STUDIO_CUSTOMERS = "STU_U.R";
  public const string VIEW_STUDIO_ARTISTS = "STU_A.R";
  public const string VIEW_STUDIO_ARTIST_SCHEDULE = "STU_AS.R";
  public const string VIEW_STUDIO_SERVICES = "STU_S.R";
  public const string VIEW_STUDIO_BOOKING = "STU_B.R";
  public const string VIEW_STUDIO_INVOICE = "STU_I.R";
  public const string VIEW_OWNED_INVOICE = "USR_I.R";

  // testimonial
  public const string MANAGE_TESTIMONIAL = "TESTI.ALL";
  public const string MANAGE_OWN_TESTIMONIAL = "TESTI.OWN";
}
