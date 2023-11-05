namespace art_tattoo_be.Application.DTOs.Shift;

public class UpdateShift
{
  public DateTime Start { get; set; }
  public DateTime End { get; set; }
  public List<Guid>? AssignArtists { get; set; }
  public List<Guid>? UnassignArtists { get; set; }
}
