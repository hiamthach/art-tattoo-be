namespace art_tattoo_be.Application.Middlewares;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using art_tattoo_be.Core.Jwt;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class ProtectedAttribute : Attribute, IAuthorizationFilter
{
  private readonly IJwtService _jwtService;

  public ProtectedAttribute()
  {
    _jwtService = new JwtService();
  }

  public void OnAuthorization(AuthorizationFilterContext context)
  {
    var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();

    if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
    {
      context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
      context.Result = new JsonResult("No token provided");
      return;
    }

    var token = authHeader.Substring("Bearer ".Length);

    try
    {
      var payload = _jwtService.ValidateToken(token);

      // add payload to context
      context.HttpContext.Items["payload"] = payload;
    }
    catch (Exception)
    {
      context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
      context.Result = new JsonResult("No token provided");
      return;
    }
  }

}
