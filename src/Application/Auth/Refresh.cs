namespace art_tattoo_be.Api.Auth;

public class RefreshReq
{
  public string RefreshToken { get; set; } = null!;
}

public class RefreshResp
{
  public string AccessToken { get; set; } = null!;
  public string RefreshToken { get; set; } = null!;
}
