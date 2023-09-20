namespace art_tattoo_be.Application.Auth;

public class LoginReq
{
  public string Email { get; set; } = null!;
  public string Password { get; set; } = null!;
}

public class LoginResp
{
  public string AccessToken { get; set; } = null!;

  public string RefreshToken { get; set; } = null!;
}