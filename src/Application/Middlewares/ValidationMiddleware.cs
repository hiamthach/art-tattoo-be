namespace art_tattoo_be.Application.Middlewares;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

public class ModelStateValidationMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<ModelStateValidationMiddleware> _logger;

  public ModelStateValidationMiddleware(RequestDelegate next, ILogger<ModelStateValidationMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    _logger.LogInformation($"api request: {context.Request.Path} [{context.Request.Method}]:");
    if (!context.Request.Path.StartsWithSegments("/api")) // Adjust the path as needed
    {
      await _next(context);
      return;
    }

    if (context.Request.Method == "POST" || context.Request.Method == "PUT") // Customize for specific HTTP methods
    {
      var modelState = context.RequestServices.GetRequiredService<ModelStateDictionary>();

      if (!modelState.IsValid)
      {
        var validationErrors = modelState.Values
            .Where(v => v.Errors.Count > 0)
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
          errors = validationErrors
        });

        return;
      }
    }

    await _next(context);
  }
}
