namespace art_tattoo_be.Application.DTOs.Auth;

public class LoginReq
{
  public string Email { get; set; } = null!;
  public string Password { get; set; } = null!;
}

public class LoginResp
{
  public string Message { get; set; } = null!;
  public TokenResp Token { get; set; } = null!;
}

public class TokenResp
{
  public string AccessToken { get; set; } = null!;
  public int AccessTokenExp { get; set; }
  public string RefreshToken { get; set; } = null!;
  public int RefreshTokenExp { get; set; }
}
