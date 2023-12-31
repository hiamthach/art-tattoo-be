namespace art_tattoo_be.Domain.Testimonial;

using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Domain.User;

public class Testimonial
{
  public Guid Id { get; set; }
  public Guid StudioId { get; set; }
  public string Title { get; set; } = null!;
  public string Content { get; set; } = null!;
  public double Rating { get; set; }
  public Guid CreatedBy { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  public virtual Studio Studio { get; set; } = null!;
  public User User { get; set; } = null!;
}
public class TestimonialList
{
  public IEnumerable<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
  public int TotalCount { get; set; }
}
