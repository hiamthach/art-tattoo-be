using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using art_tattoo_be.Domain.User;
using art_tattoo_be.Domain.RoleBase;

namespace art_tattoo_be.Infrastructure.Database;

public class ArtTattooDbContext : IdentityDbContext
{
  public DbSet<User> Users { get; set; }
  public DbSet<Role> Roles { get; set; }
  public DbSet<Permission> Permissions { get; set; }

  public ArtTattooDbContext(DbContextOptions<ArtTattooDbContext> options) : base(options) { }


  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    builder.Entity<User>(entity =>
    {
      entity.ToTable("users");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedOnAdd();
      entity.Property(e => e.Email).IsRequired();
      entity.Property(e => e.Password).IsRequired();
      entity.Property(e => e.FullName).IsRequired();
    });

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
  }
}