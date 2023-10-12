namespace art_tattoo_be.Application.DTOs.Studio;

public class UpdateStudioReq
{
  public string? Name { get; set; }

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

  public IEnumerable<StudioWorkingTimeCreate>? WorkingTimes { get; set; }
}
