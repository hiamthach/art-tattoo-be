namespace art_tattoo_be.Domain.Studio;

using art_tattoo_be.Domain.Category;

public class StudioService
{
  public Guid Id { get; set; }
  public Guid StudioId { get; set; }
  public int CategoryId { get; set; }
  public string Name { get; set; } = null!;
  public string Description { get; set; } = null!;
  public double MinPrice { get; set; }
  public double MaxPrice { get; set; }
  public double Discount { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  public Studio Studio { get; set; } = null!;
  public Category Category { get; set; } = null!;
}
