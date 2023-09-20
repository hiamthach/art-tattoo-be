namespace art_tattoo_be.Domain.RoleBase;

public class Permission
{
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;

  public string Slug { get; set; } = null!;

  public string? Description { get; set; }

  public DateTime? CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }

  public ICollection<RolePermission> RolePermission { get; set; } = null!;
}