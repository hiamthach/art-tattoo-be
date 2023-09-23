using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using art_tattoo_be.Domain.User;
using art_tattoo_be.Domain.RoleBase;
using art_tattoo_be.Domain.Category;
using art_tattoo_be.Application.Shared.Enum;

namespace art_tattoo_be.Infrastructure.Database;

public class ArtTattooDbContext : IdentityDbContext
{
  public DbSet<Role> Roles { get; set; }
  public DbSet<Permission> Permissions { get; set; }
  public DbSet<User> Users { get; set; }
  public DbSet<Category> Categories { get; set; }

  public ArtTattooDbContext(DbContextOptions<ArtTattooDbContext> options) : base(options) { }


  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    builder.Entity<Role>(entity =>
    {
      entity.ToTable("roles");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedOnAdd().UseIdentityColumn();
      entity.Property(e => e.Name).IsRequired();
      entity.Property(e => e.Description).IsRequired(false);
      entity.HasMany(e => e.Permissions).WithMany(e => e.Roles);
    });

    builder.Entity<Permission>(entity =>
    {
      entity.ToTable("permissions");
      entity.HasKey(e => e.Slug);
      entity.Property(e => e.Slug).IsRequired();
      entity.Property(e => e.Name).IsRequired();
      entity.Property(e => e.Description).IsRequired(false);
    });

    builder.Entity<User>(entity =>
    {
      entity.ToTable("users");
      entity.HasKey(e => e.Id);
      entity.HasOne(e => e.Role).WithMany().HasForeignKey(e => e.RoleId).IsRequired();
      entity.HasIndex(e => e.Email).IsUnique();
      entity.Property(e => e.Id).ValueGeneratedOnAdd();
      entity.Property(e => e.Email).IsRequired();
      entity.Property(e => e.Password).IsRequired();
      entity.Property(e => e.FullName).IsRequired();
      entity.Property(e => e.Phone).IsRequired(false);
      entity.Property(e => e.Address).IsRequired(false);
      entity.Property(e => e.Avatar).IsRequired(false);
      entity.Property(e => e.Status).IsRequired().HasDefaultValue(UserStatusEnum.Active);
      entity.Property(e => e.CreatedAt).ValueGeneratedOnAdd().HasDefaultValueSql("CURRENT_TIMESTAMP");
      entity.Property(e => e.UpdatedAt).ValueGeneratedOnUpdate().HasDefaultValueSql("CURRENT_TIMESTAMP");
    });

    builder.Entity<Category>(entity =>
    {
      entity.ToTable("tattoo_categories");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedOnAdd().UseIdentityColumn();
      entity.Property(e => e.Name).IsRequired();
      entity.Property(e => e.Description).IsRequired(false);
    });
  }
}
