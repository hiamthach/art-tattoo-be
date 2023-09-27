using Microsoft.IdentityModel.Tokens;

namespace art_tattoo_be.Core.Jwt;

public class Payload
{
  public Guid UserId { get; set; }
  public int RoleId { get; set; }
  public Guid SessionId { get; set; }
}
