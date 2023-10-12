namespace art_tattoo_be.Infrastructure.Database;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using art_tattoo_be.Domain.User;
using art_tattoo_be.Domain.RoleBase;
using art_tattoo_be.Domain.Category;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Domain.Testimonial;
using art_tattoo_be.Domain.Booking;
using art_tattoo_be.Domain.Invoice;
using art_tattoo_be.Domain.Media;
using art_tattoo_be.Application.Shared.Constant;
using art_tattoo_be.Domain.Blog;

public class ArtTattooDbContext : IdentityDbContext
{
  public new DbSet<Role> Roles { get; set; } = null!;
  public DbSet<Permission> Permissions { get; set; } = null!;
  public new DbSet<User> Users { get; set; } = null!;
  public DbSet<Category> Categories { get; set; } = null!;
  public DbSet<Studio> Studios { get; set; } = null!;
  public DbSet<StudioService> StudioServices { get; set; } = null!;
  public DbSet<StudioWorkingTime> StudioWorkingTimes { get; set; } = null!;
  public DbSet<StudioUser> StudioUsers { get; set; } = null!;
  public DbSet<Testimonial> Testimonials { get; set; } = null!;
  public DbSet<Shift> Shifts { get; set; } = null!;
  public DbSet<Appointment> Appointments { get; set; } = null!;
  public DbSet<Invoice> Invoices { get; set; } = null!;
  public DbSet<Media> Medias { get; set; } = null!;

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
      entity.HasIndex(e => e.Name).IsUnique();
      // add default record
      entity.HasData(
        new Role { Id = RoleConst.GetRoleId(RoleConst.ADMIN), Name = RoleConst.ADMIN, Description = "Admin" },
        new Role { Id = RoleConst.GetRoleId(RoleConst.SYSTEM_STAFF), Name = RoleConst.SYSTEM_STAFF, Description = "System Staff" },
        new Role { Id = RoleConst.GetRoleId(RoleConst.STUDIO_MANAGER), Name = RoleConst.STUDIO_MANAGER, Description = "Studio Manager" },
        new Role { Id = RoleConst.GetRoleId(RoleConst.STUDIO_STAFF), Name = RoleConst.STUDIO_STAFF, Description = "Studio Staff" },
        new Role { Id = RoleConst.GetRoleId(RoleConst.ARTIST), Name = RoleConst.ARTIST, Description = "Studio Artist" },
        new Role { Id = RoleConst.GetRoleId(RoleConst.MEMBER), Name = RoleConst.MEMBER, Description = "Member" }
      );
    });

    builder.Entity<Permission>(entity =>
    {
      entity.ToTable("permissions");
      entity.HasKey(e => e.Slug);
      entity.Property(e => e.Slug).IsRequired().HasMaxLength(10);
      entity.Property(e => e.Name).IsRequired().HasMaxLength(30);
      entity.Property(e => e.Description).IsRequired(false).HasMaxLength(255);
      entity.HasIndex(e => e.Slug).IsUnique();
    });

    builder.Entity<User>(entity =>
    {
      entity.ToTable("users");
      entity.HasKey(e => e.Id);
      entity.HasOne(e => e.Role).WithMany(e => e.Users).HasForeignKey(e => e.RoleId).IsRequired();
      entity.HasMany(e => e.ListMedia).WithMany(e => e.UserMedia);
      entity.HasIndex(e => e.Email).IsUnique();
      entity.Property(e => e.Id).ValueGeneratedOnAdd();
      entity.Property(e => e.Email).IsRequired().HasMaxLength(30);
      entity.Property(e => e.Password).IsRequired().HasMaxLength(100);
      entity.Property(e => e.FullName).IsRequired().HasMaxLength(30);
      entity.Property(e => e.Phone).IsRequired(false).HasMaxLength(15);
      entity.Property(e => e.Address).IsRequired(false).HasMaxLength(255);
      entity.Property(e => e.Avatar).IsRequired(false).HasMaxLength(255);
      entity.Property(e => e.RoleId).IsRequired();
      entity.Property(e => e.Status).IsRequired().HasDefaultValue(UserStatusEnum.Active).HasConversion(
        v => v.ToString(),
        v => (UserStatusEnum)Enum.Parse(typeof(UserStatusEnum), v));
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
      entity.HasMany(e => e.ListMedia).WithMany(e => e.StudioMedia);
      entity.Property(e => e.Id).ValueGeneratedOnAdd();
      entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
      entity.Property(e => e.Detail).IsRequired(false);
      entity.Property(e => e.Introduction).IsRequired(false);
      entity.Property(e => e.Slogan).IsRequired(false).HasMaxLength(50);
      entity.Property(e => e.Logo).IsRequired(false).HasMaxLength(255);
      entity.Property(e => e.Website).IsRequired(false).HasMaxLength(255);
      entity.Property(e => e.Phone).IsRequired().HasMaxLength(15);
      entity.Property(e => e.Email).IsRequired().HasMaxLength(30);
      entity.Property(e => e.Facebook).IsRequired(false).HasMaxLength(255);
      entity.Property(e => e.Instagram).IsRequired(false).HasMaxLength(255);
      entity.Property(e => e.Address).IsRequired().HasMaxLength(255);
      entity.Property(e => e.Latitude);
      entity.Property(e => e.Longitude);
      entity.Property(e => e.Status).IsRequired().HasDefaultValue(StudioStatusEnum.Inactive).HasConversion(
        v => v.ToString(),
        v => (StudioStatusEnum)Enum.Parse(typeof(StudioStatusEnum), v)
      );
      entity.Property(e => e.CreatedAt).ValueGeneratedOnAdd().HasDefaultValueSql("CURRENT_TIMESTAMP");
      entity.Property(e => e.UpdatedAt).ValueGeneratedOnUpdate().HasDefaultValueSql("CURRENT_TIMESTAMP");
      entity.HasIndex(e => e.Name);
    });

    builder.Entity<StudioService>(entity =>
    {
      entity.ToTable("studio_services");
      entity.HasMany(e => e.ListMedia).WithMany(e => e.StudioServiceMedia);
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
      entity.Property(e => e.OpenAt).HasColumnType("time").IsRequired();
      entity.Property(e => e.CloseAt).HasColumnType("time").IsRequired();
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
      entity.Property(e => e.IsDisabled).IsRequired(false);
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

    builder.Entity<Shift>(entity =>
    {
      entity.ToTable("Shifts");
      entity.HasKey(e => e.Id);
      entity.HasOne(e => e.Artist).WithMany(e => e.Shifts).HasForeignKey(e => e.ArtistId).IsRequired();
      entity.Property(e => e.Id).ValueGeneratedOnAdd();
      entity.Property(e => e.ArtistId).IsRequired();
      entity.Property(e => e.Start).IsRequired();
      entity.Property(e => e.End).IsRequired();
      entity.Property(e => e.CreatedAt).ValueGeneratedOnAdd().HasDefaultValueSql("CURRENT_TIMESTAMP");
      entity.Property(e => e.UpdatedAt).ValueGeneratedOnUpdate().HasDefaultValueSql("CURRENT_TIMESTAMP");
    });

    builder.Entity<Appointment>(entity =>
    {
      entity.ToTable("appointments");
      entity.HasMany(e => e.ListMedia).WithMany(e => e.AppointmentMedia);
      entity.HasKey(e => e.Id);
      entity.HasOne(e => e.Studio).WithMany(e => e.Appointments).HasForeignKey(e => e.StudioId).IsRequired().OnDelete(DeleteBehavior.Restrict); ;
      entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId).IsRequired().OnDelete(DeleteBehavior.Restrict);
      entity.HasOne(e => e.Shift).WithMany(e => e.Appointments).HasForeignKey(e => e.ShiftId).IsRequired().OnDelete(DeleteBehavior.Restrict);
      entity.HasOne(e => e.Artist).WithMany(e => e.Appointments).HasForeignKey(e => e.DoneBy).OnDelete(DeleteBehavior.Restrict);
      entity.Property(e => e.Id).ValueGeneratedOnAdd();
      entity.Property(e => e.StudioId).IsRequired();
      entity.Property(e => e.UserId).IsRequired();
      entity.Property(e => e.ShiftId).IsRequired();
      entity.Property(e => e.DoneBy).IsRequired(false);
      entity.Property(e => e.Notes).IsRequired(false).HasMaxLength(500);
      entity.Property(e => e.Status).IsRequired().HasDefaultValue(AppointmentStatusEnum.Pending).HasConversion(
        v => v.ToString(),
        v => (AppointmentStatusEnum)Enum.Parse(typeof(AppointmentStatusEnum), v));
      entity.Property(e => e.CreatedAt).ValueGeneratedOnAdd().HasDefaultValueSql("CURRENT_TIMESTAMP");
      entity.Property(e => e.UpdatedAt).ValueGeneratedOnUpdate().HasDefaultValueSql("CURRENT_TIMESTAMP");
    });

    builder.Entity<Invoice>(entity =>
    {
      entity.ToTable("invoices");
      entity.HasKey(e => e.Id);
      entity.HasOne(e => e.Studio).WithMany(e => e.Invoices).HasForeignKey(e => e.StudioId).IsRequired();
      entity.HasOne(e => e.User).WithMany(e => e.Invoices).HasForeignKey(e => e.UserId).IsRequired();
      entity.HasOne(e => e.Appointment).WithMany(e => e.ListInvoice).HasForeignKey(e => e.AppointmentId).IsRequired(false);
      entity.Property(e => e.Id).ValueGeneratedOnAdd();
      entity.Property(e => e.StudioId).IsRequired();
      entity.Property(e => e.AppointmentId).IsRequired(false);
      entity.Property(e => e.UserId).IsRequired();
      entity.Property(e => e.Total).IsRequired();
      entity.Property(e => e.PayMethod).IsRequired().HasConversion(
        v => v.ToString(),
        v => (PayMethodEnum)Enum.Parse(typeof(PayMethodEnum), v));
      entity.Property(e => e.Notes).IsRequired(false).HasMaxLength(500);
      entity.Property(e => e.CreatedAt).ValueGeneratedOnAdd().HasDefaultValueSql("CURRENT_TIMESTAMP");
      entity.Property(e => e.UpdatedAt).ValueGeneratedOnUpdate().HasDefaultValueSql("CURRENT_TIMESTAMP");
    });

    builder.Entity<Media>(entity =>
    {
      entity.ToTable("media");
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedOnAdd();
      entity.Property(e => e.Url).IsRequired().HasMaxLength(255);
      entity.Property(e => e.Type).IsRequired().HasConversion(
        v => v.ToString(),
        v => (MediaTypeEnum)Enum.Parse(typeof(MediaTypeEnum), v));
      entity.Property(e => e.CreatedAt).ValueGeneratedOnAdd().HasDefaultValueSql("CURRENT_TIMESTAMP");
      entity.Property(e => e.UpdatedAt).ValueGeneratedOnUpdate().HasDefaultValueSql("CURRENT_TIMESTAMP");
    });

    builder.Entity<Blog>(entity =>
    {
      entity.ToTable("blogs");
      entity.HasKey(e => e.Id);
      entity.HasOne(e => e.User).WithMany(e => e.Blogs).HasForeignKey(e => e.CreatedBy).IsRequired();
      entity.HasOne(e => e.Studio).WithMany(e => e.Blogs).HasForeignKey(e => e.StudioId).IsRequired(false);
      entity.Property(e => e.Id).ValueGeneratedOnAdd();
      entity.Property(e => e.Title).IsRequired(true).HasMaxLength(50);
      entity.Property(e => e.Slug).IsRequired(true).HasMaxLength(50);
      entity.Property(e => e.Content).IsRequired(true).HasMaxLength(2000);
      entity.Property(e => e.CreatedAt).ValueGeneratedOnAdd().HasDefaultValueSql("CURRENT_TIMESTAMP");
      entity.Property(e => e.UpdatedAt).ValueGeneratedOnUpdate().HasDefaultValueSql("CURRENT_TIMESTAMP");
    });
  }
}
