namespace art_tattoo_be.Domain.User;

public class User
{
  public Guid Id { get; set; }
  public string Email { get; set; } = null!;
  public string Password { get; set; } = null!;

  public string FullName { get; set; } = null!;

  public string? Phone { get; set; }
  public string? Address { get; set; }
  public string? Avatar { get; set; }
  public int? Role { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}