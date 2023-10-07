namespace art_tattoo_be.Domain.Blog;

using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Domain.User;

public class Blog
{
  public Guid Id { get; set; }
  public string Title { get; set; } = null!;
  public string Slug { get; set; } = null!;
  public string Content { get; set; } = null!;
  public bool IsPublish { get; set; }
  public Guid CreatedBy { get; set; }
  public Guid? StudioId { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  public Studio? Studio { get; set; }
  public User User { get; set; } = null!;
}
