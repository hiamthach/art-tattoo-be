namespace art_tattoo_be.Domain.Studio;

public class StudioUser
{
  public Guid Id { get; set; }
  public Guid StudioId { get; set; }
  public Guid UserId { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}