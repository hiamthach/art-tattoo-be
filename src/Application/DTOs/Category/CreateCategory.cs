namespace art_tattoo_be.Application.DTOs.Category;

public class CreateCategoryReq
{
  public string Name { get; set; } = null!;
  public string? Description { get; set; }
  public string? Image { get; set; }
};
