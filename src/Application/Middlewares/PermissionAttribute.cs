namespace art_tattoo_be.Application.Middlewares;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using art_tattoo_be.Core.Jwt;
using art_tattoo_be.Domain.RoleBase;
using art_tattoo_be.Application.Shared.Handler;
using art_tattoo_be.Infrastructure.Cache;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class PermissionAttribute : Attribute, IAuthorizationFilter
{
  private readonly string[] _permissions;

  public PermissionAttribute(params string[] permission)
  {
    _permissions = permission;
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
      var isForbidden = true;
      var payload = context.HttpContext.Items["payload"] as Payload;

      if (payload != null && repo != null)
      {
        if (cacheService != null)
        {
          var redisKey = $"roles:{payload.RoleId}:permissions";
          var rolePermissionsCache = await cacheService.Get<List<string>>(redisKey);

          if (rolePermissionsCache != null)
          {
            for (int i = 0; i < _permissions.Length; i++)
            {
              if (rolePermissionsCache.Contains(_permissions[i]))
              {
                isForbidden = false;
                context.HttpContext.Items["permission"] = _permissions[i];
                break;
              }
            }
          }
          else
          {
            var rolePermissions = repo.GetRolePermissionSlugs(payload.RoleId);
            if (rolePermissions != null)
            {
              await cacheService.Set(redisKey, rolePermissions, TimeSpan.FromDays(1));

              for (int i = 0; i < _permissions.Length; i++)
              {
                if (!rolePermissions.Contains(_permissions[i]))
                {
                  isForbidden = false;
                  context.HttpContext.Items["permission"] = _permissions[i];
                  break;
                }
              }
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
    }
    catch (Exception)
    {
      context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
      context.Result = new JsonResult("No token provided");
      return;
    }
  }
}
