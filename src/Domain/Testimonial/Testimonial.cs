namespace art_tattoo_be.Domain.Testimonial;

public class Testimonial
{
  public Guid Id { get; set; }
  public Guid StudioId { get; set; }
  public string Title { get; set; } = null!;
  public string Content { get; set; } = null!;
  public decimal Rating { get; set; }
  public Guid CreatedBy { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}