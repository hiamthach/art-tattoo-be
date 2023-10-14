using System.ComponentModel.DataAnnotations;
using art_tattoo_be.Application.Shared.Constant;

namespace art_tattoo_be.Application.DTOs.User;

public class UpdateUserReq
{
  public string? FullName { get; set; }
  public string? Phone { get; set; }
  public string? Address { get; set; }
  public string? Avatar { get; set; }
  public DateTime? Birthday { get; set; }
}

public class UpdatePasswordReq
{
  [Required]
  [StringLength(20, ErrorMessage = "Password must be at least 6 characters", MinimumLength = 6)]
  [RegularExpression(RegexConst.PASSWORD, ErrorMessage = "Password must contain at least 1 uppercase letter, 1 lowercase letter and 1 number")]
  public string OldPassword { get; set; } = null!;
  [Required]
  [StringLength(20, ErrorMessage = "Password must be at least 6 characters", MinimumLength = 6)]
  [RegularExpression(RegexConst.PASSWORD, ErrorMessage = "Password must contain at least 1 uppercase letter, 1 lowercase letter and 1 number")]
  public string NewPassword { get; set; } = null!;
}
