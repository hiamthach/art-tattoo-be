namespace art_tattoo_be.Domain.User;

using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.RoleBase;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Domain.Testimonial;
using art_tattoo_be.Domain.Invoice;
using art_tattoo_be.Domain.Media;
using art_tattoo_be.Domain.Blog;

public class User
{
  public Guid Id { get; set; }
  public string Email { get; set; } = null!;
  public string Password { get; set; } = null!;
  public string FullName { get; set; } = null!;
  public string? Phone { get; set; }
  public string? Address { get; set; }
  public string? Avatar { get; set; }
  public DateTime? Birthday { get; set; }
  public int RoleId { get; set; } = 1;
  public UserStatusEnum Status { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
  public DateTime LastLoginAt { get; set; }

  public Role Role { get; set; } = null!;
  public List<StudioUser> StudioUsers { get; set; } = null!;
  public List<Testimonial> Testimonials { get; set; } = new();
  public List<Invoice> Invoices { get; set; } = new();
  public List<Media> ListMedia { get; set; } = new();
  public List<Blog> Blogs { get; set; } = new();
}

public class UserList
{
  public IEnumerable<User> Users { get; set; } = new List<User>();
  public int TotalCount { get; set; }
}
