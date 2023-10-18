namespace art_tattoo_be.Application.Middlewares;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using art_tattoo_be.Core.Jwt;
using art_tattoo_be.Domain.RoleBase;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class PermissionAttribute : Attribute, IAuthorizationFilter
{
  private readonly IJwtService _jwtService;
  private readonly string _permission;
  // private readonly IRoleBaseRepository _repo;

  public PermissionAttribute(string permission)
  {
    _permission = permission;
    _jwtService = new JwtService();
  }
  public void OnAuthorization(AuthorizationFilterContext context)
  {
    var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
    var serviceProvider = context.HttpContext.RequestServices;

    var repo = serviceProvider.GetService<IRoleBaseRepository>();

    if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
    {
      context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
      context.Result = new JsonResult("No token provided");
      return;
    }

    var token = authHeader.Substring("Bearer ".Length);

    try
    {
      // var jwtService = new JwtService();
      var payload = _jwtService.ValidateToken(token);

      if (payload != null && repo != null)
      {
        var rolePermissions = repo.GetRoleById(payload.RoleId);
        if (rolePermissions != null)
        {
          context.HttpContext.Items["permission"] = rolePermissions;
        }
      }

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
