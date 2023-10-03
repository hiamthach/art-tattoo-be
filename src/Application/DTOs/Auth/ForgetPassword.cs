using System.ComponentModel.DataAnnotations;
using art_tattoo_be.Application.Shared.Constant;

namespace art_tattoo_be.Application.DTOs.Auth;

public class RequestCodeReq
{
  public string Email { get; set; } = null!;
}

public class ResetPasswordReq
{
  public string Email { get; set; } = null!;
  public string Code { get; set; } = null!;

  [Required]
  [StringLength(20, ErrorMessage = "Password must be at least 6 characters", MinimumLength = 6)]
  [RegularExpression(RegexConst.PASSWORD, ErrorMessage = "Password must contain at least 1 uppercase letter, 1 lowercase letter and 1 number")]
  public string Password { get; set; } = null!;
}
