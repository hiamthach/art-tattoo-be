namespace art_tattoo_be.Core.Jwt;

using art_tattoo_be.Application.Shared.Enum;

public class Payload
{
  public Guid UserId { get; set; }
  public int RoleId { get; set; }
  public Guid SessionId { get; set; }
  public UserStatusEnum Status { get; set; }
}
