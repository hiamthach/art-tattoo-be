using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using art_tattoo_be.Domain.User;
using art_tattoo_be.Domain.RoleBase;
using art_tattoo_be.Domain.Category;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Domain.Testimonial;

namespace art_tattoo_be.Infrastructure.Database;

public class ArtTattooDbContext : IdentityDbContext
{
  public new DbSet<Role> Roles { get; set; }
  public DbSet<Permission> Permissions { get; set; }
  public new DbSet<User> Users { get; set; }
  public DbSet<Category> Categories { get; set; }
  public DbSet<Studio> Studios { get; set; }
  public DbSet<StudioService> StudioServices { get; set; }
  public DbSet<StudioWorkingTime> StudioWorkingTimes { get; set; }
  public DbSet<StudioUser> StudioUsers { get; set; }
  public DbSet<Testimonial> Testimonials { get; set; }

  public ArtTattooDbContext(DbContextOptions<ArtTattooDbContext> options) : base(options) { }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    builder.Entity<Role>(entity =>
    {
      entity.ToTable("roles");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedOnAdd().UseIdentityColumn();
      entity.Property(e => e.Name).IsRequired().HasMaxLength(30);
      entity.Property(e => e.Description).IsRequired(false).HasMaxLength(255);
      entity.HasMany(e => e.Permissions).WithMany(e => e.Roles);
    });

    builder.Entity<Permission>(entity =>
    {
      entity.ToTable("permissions");
      entity.HasKey(e => e.Slug);
      entity.Property(e => e.Slug).IsRequired().HasMaxLength(10);
      entity.Property(e => e.Name).IsRequired().HasMaxLength(30);
      entity.Property(e => e.Description).IsRequired(false).HasMaxLength(255);
    });

    builder.Entity<User>(entity =>
    {
      entity.ToTable("users");
      entity.HasKey(e => e.Id);
      entity.HasOne(e => e.Role).WithMany(e => e.Users).HasForeignKey(e => e.RoleId).IsRequired();
      entity.HasIndex(e => e.Email).IsUnique();
      entity.Property(e => e.Id).ValueGeneratedOnAdd();
      entity.Property(e => e.Email).IsRequired().HasMaxLength(30);
      entity.Property(e => e.Password).IsRequired().HasMaxLength(20);
      entity.Property(e => e.FullName).IsRequired().HasMaxLength(30);
      entity.Property(e => e.Phone).IsRequired(false).HasMaxLength(15);
      entity.Property(e => e.Address).IsRequired(false).HasMaxLength(255);
      entity.Property(e => e.Avatar).IsRequired(false).HasMaxLength(255);
      entity.Property(e => e.RoleId).IsRequired();
      entity.Property(e => e.Status).IsRequired().HasDefaultValue(UserStatusEnum.Active);
      entity.Property(e => e.CreatedAt).ValueGeneratedOnAdd().HasDefaultValueSql("CURRENT_TIMESTAMP");
      entity.Property(e => e.UpdatedAt).ValueGeneratedOnUpdate().HasDefaultValueSql("CURRENT_TIMESTAMP");
    });

    builder.Entity<Category>(entity =>
    {
      entity.ToTable("tattoo_categories");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedOnAdd().UseIdentityColumn();
      entity.Property(e => e.Name).IsRequired().HasMaxLength(30);
      entity.Property(e => e.Description).IsRequired(false).HasMaxLength(255);
    });

    builder.Entity<Studio>(entity =>
    {
      entity.ToTable("studios");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedOnAdd();
      entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
      entity.Property(e => e.Detail).IsRequired(false);
      entity.Property(e => e.Logo).IsRequired(false).HasMaxLength(255);
      entity.Property(e => e.CreatedAt).ValueGeneratedOnAdd().HasDefaultValueSql("CURRENT_TIMESTAMP");
      entity.Property(e => e.UpdatedAt).ValueGeneratedOnUpdate().HasDefaultValueSql("CURRENT_TIMESTAMP");
    });

    builder.Entity<StudioService>(entity =>
    {
      entity.ToTable("studio_services");
      entity.HasKey(e => e.Id);
      entity.HasOne(e => e.Studio).WithMany(e => e.Services).HasForeignKey(e => e.StudioId).IsRequired();
      entity.HasOne(e => e.Category).WithMany(e => e.StudioServices).HasForeignKey(e => e.CategoryId).IsRequired();
      entity.Property(e => e.Id).ValueGeneratedOnAdd();
      entity.Property(e => e.StudioId).IsRequired();
      entity.Property(e => e.CategoryId).IsRequired();
      entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
      entity.Property(e => e.Description).IsRequired(false);
      entity.Property(e => e.MinPrice).IsRequired();
      entity.Property(e => e.MaxPrice).IsRequired();
      entity.Property(e => e.Discount).IsRequired();
      entity.Property(e => e.CreatedAt).ValueGeneratedOnAdd().HasDefaultValueSql("CURRENT_TIMESTAMP");
      entity.Property(e => e.UpdatedAt).ValueGeneratedOnUpdate().HasDefaultValueSql("CURRENT_TIMESTAMP");
    });

    builder.Entity<StudioWorkingTime>(entity =>
    {
      entity.ToTable("studio_working_time");
      entity.HasKey(e => e.Id);
      entity.HasOne(e => e.Studio).WithMany(e => e.WorkingTimes).HasForeignKey(e => e.StudioId).IsRequired();
      entity.Property(e => e.Id).ValueGeneratedOnAdd();
      entity.Property(e => e.StudioId).IsRequired();
      entity.Property(e => e.DayOfWeek).IsRequired();
      entity.Property(e => e.OpenAt).IsRequired();
      entity.Property(e => e.CloseAt).IsRequired();
    });

    builder.Entity<StudioUser>(entity =>
    {
      entity.ToTable("studio_users");
      entity.HasKey(e => e.Id);
      entity.HasOne(e => e.Studio).WithMany(e => e.StudioUsers).HasForeignKey(e => e.StudioId).IsRequired();
      entity.HasOne(e => e.User).WithOne(e => e.StudioUser).HasForeignKey<StudioUser>(e => e.UserId).IsRequired();
      entity.Property(e => e.Id).ValueGeneratedOnAdd();
      entity.Property(e => e.StudioId).IsRequired();
      entity.Property(e => e.UserId).IsRequired();
      entity.Property(e => e.CreatedAt).ValueGeneratedOnAdd().HasDefaultValueSql("CURRENT_TIMESTAMP");
      entity.Property(e => e.UpdatedAt).ValueGeneratedOnUpdate().HasDefaultValueSql("CURRENT_TIMESTAMP");
    });

    builder.Entity<Testimonial>(entity =>
    {
      entity.ToTable("testimonials");
      entity.HasKey(e => e.Id);
      entity.HasOne(e => e.Studio).WithMany(e => e.Testimonials).HasForeignKey(e => e.StudioId).IsRequired();
      entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.CreatedBy).IsRequired();
      entity.Property(e => e.Id).ValueGeneratedOnAdd();
      entity.Property(e => e.StudioId).IsRequired();
      entity.Property(e => e.Title).IsRequired().HasMaxLength(50);
      entity.Property(e => e.Content).IsRequired().HasMaxLength(1000);
      entity.Property(e => e.Rating).IsRequired();
      entity.Property(e => e.CreatedBy).IsRequired();
      entity.Property(e => e.CreatedAt).ValueGeneratedOnAdd().HasDefaultValueSql("CURRENT_TIMESTAMP");
      entity.Property(e => e.UpdatedAt).ValueGeneratedOnUpdate().HasDefaultValueSql("CURRENT_TIMESTAMP");
    });
  }
}
