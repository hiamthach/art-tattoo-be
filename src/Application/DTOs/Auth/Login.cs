namespace art_tattoo_be.Application.DTOs.Auth;

using System.ComponentModel.DataAnnotations;
using art_tattoo_be.Application.Shared.Constant;

public class LoginReq
{
  [Required]
  [StringLength(30)]
  [RegularExpression(RegexConst.EMAIL, ErrorMessage = "Invalid email address")]
  public string Email { get; set; } = null!;
  [Required]
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
  public long AccessTokenExp { get; set; }
  public string RefreshToken { get; set; } = null!;
  public long RefreshTokenExp { get; set; }
}
