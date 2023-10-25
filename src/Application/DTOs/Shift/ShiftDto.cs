namespace art_tattoo_be.Application.DTOs.Shift;

public class ShiftDto
{
  public Guid Id { get; set; }
  public Guid ArtistId { get; set; }
  public DateTime Start { get; set; }
  public DateTime End { get; set; }
}
