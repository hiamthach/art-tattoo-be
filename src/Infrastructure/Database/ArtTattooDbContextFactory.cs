using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace art_tattoo_be.Infrastructure.Database;

public class ArtTattooDbContextFactory : IDesignTimeDbContextFactory<ArtTattooDbContext>
{
  public ArtTattooDbContext CreateDbContext(string[] args)
  {
    var configuration = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json")
      .Build();

    Console.WriteLine($"Using ConnectionString: {configuration.GetConnectionString("DatabaseConnection")}");

    var optionsBuilder = new DbContextOptionsBuilder<ArtTattooDbContext>();
    optionsBuilder.UseMySQL(configuration.GetConnectionString("DatabaseConnection") ?? "");

    return new ArtTattooDbContext(optionsBuilder.Options);
  }
}
