namespace art_tattoo_be.Domain.Studio;

public class StudioService
{
  public Guid Id { get; set; }
  public Guid StudioId { get; set; }
  public int CategoryId { get; set; }
  public string Name { get; set; } = null!;
  public string Description { get; set; } = null!;
  public decimal MinPrice { get; set; }
  public decimal MaxPrice { get; set; }
  public decimal Discount { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}