namespace art_tattoo_be.Application.Shared.Handler;

using art_tattoo_be.Application.Shared.Constant;
using Microsoft.AspNetCore.Mvc;

public static class ErrorResp
{
  public static IActionResult UnknownError(string message)
  {
    return new JsonResult(new { Error = message }) { StatusCode = HttpCode.UNKNOWN_ERROR };
  }

  public static IActionResult NotFound(string message)
  {
    return new JsonResult(new { Error = message }) { StatusCode = HttpCode.NOT_FOUND };
  }

  public static IActionResult BadRequest(string message)
  {
    return new JsonResult(new { Error = message }) { StatusCode = HttpCode.BAD_REQUEST };
  }

  public static IActionResult Unauthorized(string message)
  {
    return new JsonResult(new { Error = message }) { StatusCode = HttpCode.UNAUTHORIZED };
  }

  public static IActionResult SomethingWrong(string message)
  {
    return new JsonResult(new { Error = message }) { StatusCode = HttpCode.SOMETHING_WRONG };
  }

  public static IActionResult Forbidden(string message)
  {
    return new JsonResult(new { Error = message }) { StatusCode = HttpCode.FORBIDDEN };
  }
}
