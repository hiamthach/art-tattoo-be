using art_tattoo_be.Application.Shared.Enum;

namespace art_tattoo_be.Domain.Media;

public class Media
{
  public Guid Id { get; set; }
  public string Url { get; set; } = null!;
  public MediaTypeEnum Type { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}