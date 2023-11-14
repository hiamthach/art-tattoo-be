namespace art_tattoo_be.Application.DTOs.Analytics;

using art_tattoo_be.Domain.Studio;

public class StudioMostPopularArtist
{
  public Guid ArtistId { get; set; }
  public int TotalBooking { get; set; }
  public double TotalRevenue { get; set; }
  public StudioUser? Artist { get; set; }
}
