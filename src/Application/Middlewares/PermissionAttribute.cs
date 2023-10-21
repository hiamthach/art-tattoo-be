namespace art_tattoo_be.Application.Middlewares;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using art_tattoo_be.Core.Jwt;
using art_tattoo_be.Domain.RoleBase;
using art_tattoo_be.Application.Shared.Handler;
using art_tattoo_be.Infrastructure.Cache;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class PermissionAttribute : System.Attribute, IAuthorizationFilter
{
  private readonly string _permission;

  public PermissionAttribute(string permission)
  {
    _permission = permission;
  }

  public void OnAuthorization(AuthorizationFilterContext context)
  {
    Task.Run(async () => await OnAuthorizationAsync(context)).Wait();
  }

  public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
  {
    var serviceProvider = context.HttpContext.RequestServices;

    var repo = serviceProvider.GetService<IRoleBaseRepository>();
    var cacheService = serviceProvider.GetService<ICacheService>();
    try
    {
      var isForbidden = false;
      var payload = context.HttpContext.Items["payload"] as Payload;

      if (payload != null && repo != null)
      {
        if (cacheService != null)
        {
          var redisKey = $"role:{payload.RoleId}:permissions";
          var rolePermissionsCache = await cacheService.Get<List<string>>(redisKey);

          if (rolePermissionsCache != null)
          {
            if (!rolePermissionsCache.Contains(_permission))
            {
              isForbidden = true;
            }
          }
          else
          {
            var rolePermissions = repo.GetRolePermissionSlugs(payload.RoleId);
            if (rolePermissions != null)
            {
              await cacheService.Set(redisKey, rolePermissions, TimeSpan.FromDays(1));
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
      else
      {
        context.HttpContext.Items["permission"] = _permission;
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
