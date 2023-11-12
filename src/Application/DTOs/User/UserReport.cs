using System.ComponentModel.DataAnnotations;
using art_tattoo_be.Application.Shared.Constant;

namespace art_tattoo_be.Application.DTOs.User;

public class UserReport
{
  public Guid Id { get; set; }

  public string FullName { get; set; } = null!;

  [Required]
  [StringLength(30)]
  [RegularExpression(RegexConst.EMAIL, ErrorMessage = "Invalid email address")]
  public string Email { get; set; } = null!; [Required]
  [StringLength(15, ErrorMessage = "Phone number must be 0-15 characters")]
  [RegularExpression(RegexConst.PHONE_NUMBER, ErrorMessage = "Invalid phone number")]
  public string PhoneNumber { get; set; } = null!;

  public string RedirectUrl { get; set; } = null!;
}
