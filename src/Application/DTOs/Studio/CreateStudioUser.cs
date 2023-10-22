using System.ComponentModel.DataAnnotations;
using art_tattoo_be.Application.Shared.Constant;
using FirebaseAdmin.Messaging;

namespace art_tattoo_be.Application.DTOs.Studio;

public class CreateStudioUser
{
  [Required]
  public string Email { get; set; } = null!;
  [Required]
  public Guid StudioId { get; set; }
  [Required]
  [Range(RoleConst.STUDIO_MANAGER_ID, RoleConst.ARTIST_ID, ErrorMessage = "Role is not valid")]
  public int RoleId { get; set; }
}
