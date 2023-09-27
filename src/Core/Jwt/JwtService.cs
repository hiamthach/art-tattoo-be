namespace art_tattoo_be.Core.Jwt;

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public interface IJwtService
{
  string GenerateToken(Guid userId, Guid sessionId, int roleId, int exp);
  Payload? ValidateToken(string token);
}

public class JwtService : IJwtService
{
  private readonly byte[] _key;
  private readonly JwtSecurityTokenHandler _handler;
  public JwtService()
  {
    var SecretKey = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "ArtSecret";
    _key = Encoding.ASCII.GetBytes(SecretKey);
    _handler = new JwtSecurityTokenHandler();
  }

  public string GenerateToken(Guid userId, Guid sessionId, int roleId, int exp)
  {
    var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET") ?? "ArtSecret");
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(new Claim[]
      {
        new(ClaimTypes.NameIdentifier, userId.ToString()),
        new(ClaimTypes.Role, roleId.ToString()),
        new(ClaimTypes.Sid, sessionId.ToString())
      }),
      Expires = DateTime.UtcNow.AddSeconds(exp),
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };

    var token = _handler.CreateToken(tokenDescriptor);

    return _handler.WriteToken(token);
  }

  public Payload? ValidateToken(string token)
  {
    try
    {
      _handler.ValidateToken(token, new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(_key),
        ValidateIssuer = false,
        ValidateAudience = false,
        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
        ClockSkew = TimeSpan.Zero
      }, out SecurityToken validatedToken);

      var result = (JwtSecurityToken)validatedToken;

      return new Payload
      {
        UserId = Guid.Parse(result.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value),
        RoleId = int.Parse(result.Claims.First(x => x.Type == ClaimTypes.Role).Value),
        SessionId = Guid.Parse(result.Claims.First(x => x.Type == ClaimTypes.Sid).Value)
      };
    }
    catch
    {
      return null;
    }
  }
}
