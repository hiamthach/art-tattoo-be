namespace art_tattoo_be.Domain.Studio;

public class StudioWorkingTime
{
  public Guid Id { get; set; }
  public Guid StudioId { get; set; }
  public int DayOfWeek { get; set; }
  public DateTime OpenAt { get; set; }
  public DateTime CloseAt { get; set; }
}