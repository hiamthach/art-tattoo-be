namespace art_tattoo_be.Application.Controllers;

using Microsoft.AspNetCore.Mvc;
using art_tattoo_be.Infrastructure.Database;
using art_tattoo_be.Application.Shared.Handler;
using art_tattoo_be.Application.DTOs.Media;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Core.GCS;
using art_tattoo_be.Application.Shared.Constant;

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
  public async Task<IActionResult> UploadFile([FromForm] IFormFile file, [FromForm] int type)
  {
    _logger.LogInformation("Upload file:");

    try
    {
      if (file == null || file.Length == 0)
      {
        return ErrorResp.BadRequest("File is empty");
      }

      var contentType = file.ContentType;

      // check file type is image
      if (type == (int)MediaTypeEnum.Image)
      {
        if (!FileConst.IMAGE_CONTENT_TYPES.Contains(contentType))
        {
          return ErrorResp.BadRequest("File is not image type (jpg, png, gif, webp)");
        }
        else if (file.Length > FileConst.MAX_IMAGE_SIZE)
        {
          return ErrorResp.BadRequest("File is too large, max size is 5MB");
        }
      }
      else if (type == (int)MediaTypeEnum.Video)
      {
        if (!FileConst.VIDEO_CONTENT_TYPES.Contains(contentType))
        {
          return ErrorResp.BadRequest("File is not video type (mp4, avi, mov, wmv, flv, mkv)");
        }
        else if (file.Length > FileConst.MAX_VIDEO_SIZE)
        {
          return ErrorResp.BadRequest("File is too large, max size is 20MB");
        }
      }
      else if (type == (int)MediaTypeEnum.Cert)
      {
        if (!FileConst.CERT_CONTENT_TYPES.Contains(contentType))
        {
          return ErrorResp.BadRequest("File is not certificate type (pdf, doc, docx)");
        }
        else if (file.Length > FileConst.MAX_CERT_SIZE)
        {
          return ErrorResp.BadRequest("File is too large max size is 5MB");
        }
      }
      else
      {
        return ErrorResp.BadRequest("File type is invalid");
      }

      // generate file name: GUID + file extension
      var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

      // upload file to GCS
      var fileStream = file.OpenReadStream();

      var downloadUrl = await _gcsService.UploadFileAsync(fileStream, fileName, contentType);

      if (downloadUrl == null)
      {
        return ErrorResp.SomethingWrong("Error uploading file");
      }

      return Ok(new
      {
        success = true,
        url = downloadUrl
      });
    }
    catch (Exception e)
    {
      _logger.LogError(e.Message);
      return ErrorResp.SomethingWrong(e.Message);
    }
  }
}

