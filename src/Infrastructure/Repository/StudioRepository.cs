using art_tattoo_be.Application.DTOs.Pagination;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Infrastructure.Database;

namespace art_tattoo_be.Infrastructure.Repository;

public class StudioRepository : IStudioRepository
{
  private readonly ArtTattooDbContext _dbContext;

  public StudioRepository(ArtTattooDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public Task<int> CreateAsync(Studio studio)
  {
    throw new NotImplementedException();
  }

  public int DeleteStudio(Guid id)
  {
    throw new NotImplementedException();
  }

  public Task<Studio?> GetAsync(Guid id)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<Studio>> GetStudioPages(PaginationReq req)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<Studio>> GetStudios()
  {
    throw new NotImplementedException();
  }

  public Task<int> UpdateAsync(Studio studio)
  {
    throw new NotImplementedException();
  }

  public int UpdateStudioStatus(Guid id, StudioStatusEnum status)
  {
    throw new NotImplementedException();
  }
}
