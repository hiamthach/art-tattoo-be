namespace art_tattoo_be.src.Application.DTOs.StudioService;

using art_tattoo_be.Application.DTOs.Media;

public class UpdateStudioServiceReq
{
  public int? CategoryId { get; set; }
  public string? Name { get; set; } = null!;
  public string? Description { get; set; } = null!;
  public double? MinPrice { get; set; }
  public double? MaxPrice { get; set; }
  public double? Discount { get; set; }
  public bool? IsDisabled { get; set; }
  public string? Thumbnail { get; set; } = null!;
  public TimeSpan? ExpectDuration { get; set; }
  public IEnumerable<string>? ListRemoveMedia { get; set; }
  public IEnumerable<MediaCreate>? ListNewMedia { get; set; }
}
