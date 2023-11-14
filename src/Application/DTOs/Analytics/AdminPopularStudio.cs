namespace art_tattoo_be.Application.DTOs.Analytics;

using art_tattoo_be.Domain.Studio;

public class AdminMostPopularStudio
{
  public Guid StudioId { get; set; }
  public int TotalBooking { get; set; }
  public double TotalRevenue { get; set; }
  public Studio? Studio { get; set; }
}
