using System.ComponentModel.DataAnnotations;
using art_tattoo_be.Application.Shared.Constant;

namespace art_tattoo_be.Application.DTOs.Studio;

public class UpdateStudioUserReq
{
  public bool? IsDisabled { get; set; }
  [Range(RoleConst.STUDIO_MANAGER_ID, RoleConst.ARTIST_ID, ErrorMessage = "Role is not valid")]
  public int? RoleId { get; set; }
}

public class UpdateStudioUserData
{
  public bool? IsDisabled { get; set; }
  public int? RoleId { get; set; }
  public int SelfRoleId { get; set; }
}
