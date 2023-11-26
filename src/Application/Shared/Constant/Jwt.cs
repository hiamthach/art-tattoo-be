namespace art_tattoo_be.Application.Shared.Constant;

public static class JwtConst
{
  public const int ACCESS_TOKEN_EXP = 15 * 60; // 15m
  public const int REFRESH_TOKEN_EXP = 3600 * 24 * 30; // 30 days
}
