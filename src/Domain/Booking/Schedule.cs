namespace art_tattoo_be.Domain.Booking;

public class Schedule
{
  public Guid Id { get; set; }
  public Guid StudioId { get; set; }
  public Guid ArtistId { get; set; }
  public DateTime Start { get; set; }
  public DateTime End { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}
