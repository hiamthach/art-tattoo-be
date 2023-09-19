using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using art_tattoo_be.Domain.User;

namespace art_tattoo_be.Infrastructure.Database;

public class MyDbContext : IdentityDbContext
{
  public DbSet<User> Users { get; set; }

  public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }


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
  }
}