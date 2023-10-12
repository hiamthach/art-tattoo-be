using System.ComponentModel.DataAnnotations;
using art_tattoo_be.Application.DTOs.Media;
using art_tattoo_be.Application.Shared.Constant;

namespace art_tattoo_be.Application.DTOs.Studio;

public class CreateStudioReq
{
  public string Name { get; set; } = null!;

  public string? Detail { get; set; }
  public string? Logo { get; set; }
  [Required]
  [StringLength(15, ErrorMessage = "Phone number must be 0-15 characters")]
  [RegularExpression(RegexConst.PHONE_NUMBER, ErrorMessage = "Invalid phone number")]
  public string Phone { get; set; } = null!;
  [Required]
  [StringLength(30, ErrorMessage = "Email too long")]
  [RegularExpression(RegexConst.EMAIL, ErrorMessage = "Invalid email address")]
  public string Email { get; set; } = null!;
  public string? Website { get; set; }
  public string? Facebook { get; set; }
  public string? Instagram { get; set; }
  public string Address { get; set; } = null!;
  public double Latitude { get; set; }
  public double Longitude { get; set; }

  public IEnumerable<StudioWorkingTimeCreate> WorkingTimes { get; set; } = new List<StudioWorkingTimeCreate>();
  public IEnumerable<MediaCreate> ListMedia { get; set; } = new List<MediaCreate>();
}
