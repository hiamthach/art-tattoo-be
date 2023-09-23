namespace art_tattoo_be.Domain.User;

using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.RoleBase;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Domain.Testimonial;


public class User
{
  public Guid Id { get; set; }
  public string Email { get; set; } = null!;
  public string Password { get; set; } = null!;
  public string FullName { get; set; } = null!;
  public string? Phone { get; set; }
  public string? Address { get; set; }
  public string? Avatar { get; set; }
  public int RoleId { get; set; } = 1;
  public UserStatusEnum Status { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
  public DateTime LastLoginAt { get; set; }

  public Role Role { get; set; } = null!;
  public StudioUser StudioUser { get; set; } = null!;
  public List<Testimonial> Testimonials { get; set; } = new();
}
