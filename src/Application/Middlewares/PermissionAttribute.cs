namespace art_tattoo_be.Application.Middlewares;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using art_tattoo_be.Core.Jwt;
using art_tattoo_be.Domain.RoleBase;
using art_tattoo_be.Application.Shared.Handler;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class PermissionAttribute : Attribute, IAuthorizationFilter
{
  private readonly string _permission;
  // private readonly IRoleBaseRepository _repo;

  public PermissionAttribute(string permission)
  {
    _permission = permission;
  }
  public void OnAuthorization(AuthorizationFilterContext context)
  {
    var serviceProvider = context.HttpContext.RequestServices;

    var repo = serviceProvider.GetService<IRoleBaseRepository>();
    try
    {
      var isForbidden = false;
      var payload = context.HttpContext.Items["payload"] as Payload;

      if (payload != null && repo != null)
      {
        var rolePermissions = repo.GetRolePermissionSlugs(payload.RoleId);
        if (rolePermissions != null)
        {
          if (!rolePermissions.Contains(_permission))
          {
            isForbidden = true;
          }
        }
        else
        {
          isForbidden = true;
        }
      }
      else
      {
        isForbidden = true;
      }

      if (isForbidden)
      {
        context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
        context.Result = ErrorResp.Forbidden("Permission denied");
        return;
      }
    }
    catch (Exception)
    {
      context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
      context.Result = new JsonResult("No token provided");
      return;
    }
  }
}
