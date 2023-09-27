namespace art_tattoo_be.Application.DTOs.Auth;

public class RegisterReq  // Path: src/Api/Auth/RegisterReq.cs
{
  public string Email { get; set; } = null!;
  public string Password { get; set; } = null!;
  public string FullName { get; set; } = null!;
  public string PhoneNumber { get; set; } = null!;
}

public class RegisterResp
{
  public string Message { get; set; } = null!;
  public TokenResp Token { get; set; } = null!;
}
