namespace art_tattoo_be.Application.DTOs.Studio;

public class CreateStudioReq
{
  public string Name { get; set; } = null!;

  public string? Detail { get; set; }
  public string? Logo { get; set; }
  public string? Phone { get; set; }
  public string? Email { get; set; }
  public string? Website { get; set; }
  public string? Facebook { get; set; }
  public string? Instagram { get; set; }
  public string? Address { get; set; }
  public double Latitude { get; set; }
  public double Longitude { get; set; }
  /* The line `public IEnumerable<StudioWorkingTimeCreate> WorkingTimes { get; set; };` is declaring a
  property named `WorkingTimes` of type `IEnumerable<StudioWorkingTimeCreate>`. */
  public IEnumerable<StudioWorkingTimeCreate> WorkingTimes { get; set; } = new List<StudioWorkingTimeCreate>();
}
