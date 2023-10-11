namespace art_tattoo_be.Domain.Studio;

public class StudioWorkingTime
{
  public Guid Id { get; set; }
  public Guid StudioId { get; set; }
  public int DayOfWeek { get; set; }
  public TimeSpan OpenAt { get; set; }
  public TimeSpan CloseAt { get; set; }
  public Studio Studio { get; set; } = null!;
}
