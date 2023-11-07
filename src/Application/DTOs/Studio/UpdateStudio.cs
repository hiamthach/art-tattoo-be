using art_tattoo_be.Application.DTOs.Media;
using art_tattoo_be.Application.Shared.Enum;

namespace art_tattoo_be.Application.DTOs.Studio;

public class UpdateStudioReq
{
  public string? Name { get; set; }

  public string? Slogan { get; set; }
  public string? Introduction { get; set; }
  public string? Detail { get; set; }
  public string? Logo { get; set; }
  public string? Phone { get; set; }
  public string? Email { get; set; }
  public string? Website { get; set; }
  public string? Facebook { get; set; }
  public string? Instagram { get; set; }
  public string? Address { get; set; }
  public double? Latitude { get; set; }
  public double? Longitude { get; set; }
  public StudioStatusEnum? Status { get; set; }

  public IEnumerable<string>? ListRemoveMedia { get; set; }
  public IEnumerable<StudioWorkingTimeCreate>? WorkingTimes { get; set; }
  public IEnumerable<MediaCreate>? ListNewMedia { get; set; }
}
