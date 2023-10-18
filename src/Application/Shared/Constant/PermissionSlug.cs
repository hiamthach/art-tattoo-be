namespace art_tattoo_be.Application.Shared;

public static class PermissionSlugConst
{
  public const string MANAGE_USERS = "USR.ALL";
  public const string MANAGE_ROLE = "ROLE.ALL";
  public const string MANAGE_PERMISSION = "PERMISSION.ALL";
  public const string MANAGE_CATEGORY = "CATEGORY.ALL";
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
  public const string VIEW_STUDIO_CUSTOMERS = "STU_U.READ";
  public const string VIEW_STUDIO_ARTISTS = "STU_A.READ";
  public const string VIEW_STUDIO_ARTIST_SCHEDULE = "STU_AS.READ";
  public const string VIEW_STUDIO_SERVICES = "STU_S.READ";
  public const string VIEW_STUDIO_BOOKING = "STU_B.READ";
  public const string VIEW_STUDIO_INVOICE = "STU_I.READ";
  public const string VIEW_OWNED_INVOICE = "USR_I.READ";

  // testimonial
  public const string MANAGE_TESTIMONIAL = "TESTIMONIAL.ALL";
  public const string MANAGE_OWN_TESTIMONIAL = "TESTIMONIAL.OWN";
}
