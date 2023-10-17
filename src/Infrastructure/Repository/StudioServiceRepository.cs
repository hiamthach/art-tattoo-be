using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Infrastructure.Database;
using art_tattoo_be.src.Domain.Studio;

namespace art_tattoo_be.src.Infrastructure.Repository
{
  public class StudioServiceRepository : IStudioServiceRepository
  {
    private readonly ArtTattooDbContext _dbContext;

    public StudioServiceRepository(ArtTattooDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public int CreateStudioService(StudioService studioService)
    {
        _dbContext.StudioServices.Add(studioService);
        return _dbContext.SaveChanges();
    }

    public int DeleteStudioService(int id)
    {
        var studioService = _dbContext.StudioServices.Find(id) ?? throw new Exception("Studio Service not found");
        _dbContext.Remove(studioService);
        return _dbContext.SaveChanges();
    }

    public IEnumerable<StudioService> GetAll()
    {
      return _dbContext.StudioServices.ToList();
    }

    public StudioService GetById(int id)
    {
      return _dbContext.StudioServices.Find(id) ?? throw new Exception("Studio Service not found");
    }
  }
}