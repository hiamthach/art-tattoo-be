namespace art_tattoo_be.Application.Auth;

public class RegisterReq  // Path: src/Api/Auth/RegisterReq.cs
{
  public string Email { get; set; } = null!;
  public string Password { get; set; } = null!;
  public string FullName { get; set; } = null!;
  public string PhoneNumber { get; set; } = null!;
}

public class RegisterResp
{
  public string AccessToken { get; set; } = null!;
  public string RefreshToken { get; set; } = null!;
}