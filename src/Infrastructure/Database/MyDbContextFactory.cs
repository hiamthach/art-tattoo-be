using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace art_tattoo_be.Infrastructure.Database;

public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
{
  public MyDbContext CreateDbContext(string[] args)
  {
    var configuration = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json")
      .Build();

    var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
    optionsBuilder.UseSqlServer(configuration.GetConnectionString("DatabaseConnection"));

    return new MyDbContext(optionsBuilder.Options);
  }
}
