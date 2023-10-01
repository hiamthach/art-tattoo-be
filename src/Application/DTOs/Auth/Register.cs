namespace art_tattoo_be.Application.DTOs.Auth;

using System.ComponentModel.DataAnnotations;
using art_tattoo_be.Application.Shared.Constant;

public class RegisterReq  // Path: src/Api/Auth/RegisterReq.cs
{
  [Required]
  [StringLength(30)]
  [RegularExpression(RegexConst.EMAIL, ErrorMessage = "Invalid email address")]
  public string Email { get; set; } = null!;

  [Required]
  [StringLength(20, ErrorMessage = "Password must be at least 6 characters", MinimumLength = 6)]
  [RegularExpression(RegexConst.PASSWORD, ErrorMessage = "Password must contain at least 1 uppercase letter, 1 lowercase letter and 1 number")]
  public string Password { get; set; } = null!;

  [Required]
  [StringLength(30)]
  public string FullName { get; set; } = null!;

  public string PhoneNumber { get; set; } = null!;
}
