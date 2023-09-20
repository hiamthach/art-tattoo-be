using art_tattoo_be.Infrastructure.Database;

namespace art_tattoo_be.Infrastructure.Repositories;

internal abstract class BaseRepository<TEntity>
{
  protected readonly ArtTattooDbContext _dbContext;

  protected BaseRepository(ArtTattooDbContext dbContext)
  {
    _dbContext = dbContext;
  }
}