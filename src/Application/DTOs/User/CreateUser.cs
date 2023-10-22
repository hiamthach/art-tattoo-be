using System.ComponentModel.DataAnnotations;
using art_tattoo_be.Application.Shared.Constant;

namespace art_tattoo_be.Application.DTOs.User;

public class CreateUserReq
{
  public string Email { get; set; } = null!;
  public string FullName { get; set; } = null!;
  public string Password { get; set; } = null!;
  public string? Phone { get; set; }
  public string? Address { get; set; }
  public string? Avatar { get; set; }
  public DateTime? Birthday { get; set; }
  [Required]
  [Range(RoleConst.SYSTEM_STAFF_ID, RoleConst.MEMBER_ID, ErrorMessage = "Invalid role")]
  public int RoleId { get; set; }
}
