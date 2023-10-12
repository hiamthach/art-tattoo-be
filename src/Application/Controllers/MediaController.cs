namespace art_tattoo_be.Application.Controllers;

using Microsoft.AspNetCore.Mvc;
using art_tattoo_be.Infrastructure.Database;
using art_tattoo_be.Application.Shared.Handler;
using art_tattoo_be.Application.DTOs.Media;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Core.GCS;

[Produces("application/json")]
[ApiController]
[Route("api/media")]
public class MediaController : ControllerBase
{
  private readonly ILogger<MediaController> _logger;
  private readonly IGCSService _gcsService;

  // private readonly IMediaRepository _mediaRepo;

  public MediaController(ILogger<MediaController> logger, ArtTattooDbContext dbContext, IGCSService gcsService)
  {
    _logger = logger;
    _gcsService = gcsService;
    // _mediaRepo = new MediaRepository(dbContext);
  }

  [HttpGet("type")]
  public IActionResult GetMediaTypes()
  {
    _logger.LogInformation("Get media types:");

    var mediaTypes = new List<MediaType>();
    foreach (var value in Enum.GetValues(typeof(MediaTypeEnum)))
    {
      if (value != null)
      {
        var name = value.ToString();
        if (name != null)
        {
          mediaTypes.Add(new MediaType { Name = name, Value = (int)value });
        }
      }
    }

    return Ok(mediaTypes);
  }

  [HttpPost("upload")]
  public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
  {
    _logger.LogInformation("Upload file:");

    try
    {
      if (file == null || file.Length == 0)
      {
        return ErrorResp.BadRequest("File is empty");
      }

      var fileName = file.FileName;
      var contentType = file.ContentType;
      var fileStream = file.OpenReadStream();

      var downloadUrl = await _gcsService.UploadFileAsync(fileStream, fileName, contentType);

      if (downloadUrl == null)
      {
        return ErrorResp.SomethingWrong("Error uploading file");
      }

      return Ok(downloadUrl);
    }
    catch (Exception e)
    {
      _logger.LogError(e.Message);
      return ErrorResp.SomethingWrong(e.Message);
    }
  }
}

