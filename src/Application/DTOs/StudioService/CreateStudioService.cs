namespace art_tattoo_be.src.Application.DTOs.StudioService;

using System;
using System.ComponentModel.DataAnnotations;
using art_tattoo_be.Application.DTOs.Media;

public class CreateStudioServiceReq
{
  public int CategoryId { get; set; }
  public string Name { get; set; } = null!;
  public string Description { get; set; } = null!;
  [Required]
  [Range(1, double.MaxValue, ErrorMessage = "Min Price must larger than 0")]
  public double MinPrice { get; set; }
  [Required]
  [Range(1, double.MaxValue, ErrorMessage = "Max Price must larger than 0")]
  public double MaxPrice { get; set; }
  public TimeSpan ExpectDuration { get; set; }
  public string Thumbnail { get; set; } = null!;
  public double Discount { get; set; }
  public List<MediaCreate> ListMedia { get; set; } = null!;
}
