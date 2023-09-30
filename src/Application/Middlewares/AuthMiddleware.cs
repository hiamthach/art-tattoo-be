namespace art_tattoo_be.Application.Middlewares;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using art_tattoo_be.Core.Jwt;

public class AuthMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<AuthMiddleware> _logger;
  private readonly IJwtService _jwtService;

  public AuthMiddleware(RequestDelegate next, ILogger<AuthMiddleware> logger, IJwtService jwtService)
  {
    _next = next;
    _logger = logger;
    _jwtService = jwtService;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    _logger.LogInformation("AuthMiddleware");

    if (!HasCustomAuthorizeAttribute(context))
    {
      await _next(context);
      return;
    }

    var authHeader = context.Request.Headers["Authorization"].ToString();

    if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
    {
      context.Response.StatusCode = StatusCodes.Status401Unauthorized;
      await context.Response.WriteAsync("Unauthorized");
      return;
    }

    var token = authHeader.Substring("Bearer ".Length);

    try
    {
      var payload = _jwtService.ValidateToken(token);

      // add payload to context
      context.Items["payload"] = payload;

      await _next(context);
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Error validating token");
      context.Response.StatusCode = StatusCodes.Status401Unauthorized;
      await context.Response.WriteAsync("Unauthorized");
    }
  }
  private bool HasCustomAuthorizeAttribute(HttpContext context)
  {
    var endpoint = context.GetEndpoint();
    if (endpoint != null)
    {
      var hasCustomAuthorizeAttribute = endpoint.Metadata
          .Any(metadata => metadata is ProtectedAttribute);
      return hasCustomAuthorizeAttribute;
    }

    return false;
  }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class ProtectedAttribute : Attribute
{

}
