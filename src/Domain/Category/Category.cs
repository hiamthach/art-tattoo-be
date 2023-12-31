namespace art_tattoo_be.Domain.Category;

using art_tattoo_be.Domain.Studio;

public class Category
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public string? Description { get; set; }
  public string? Image { get; set; }

  public List<StudioService> StudioServices { get; } = new();
}
