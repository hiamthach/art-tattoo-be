using art_tattoo_be.Application.DTOs.Pagination;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace art_tattoo_be.Infrastructure.Repository;

public class StudioRepository : IStudioRepository
{
  private readonly ArtTattooDbContext _dbContext;

  public StudioRepository(ArtTattooDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public int Count()
  {
    return _dbContext.Studios.Where(stu => stu.Status == StudioStatusEnum.Active).Count();
  }

  public async Task<int> CreateAsync(Studio studio)
  {
    await _dbContext.Studios.AddAsync(studio);
    return await _dbContext.SaveChangesAsync();
  }

  public int DeleteStudio(Guid id)
  {
    var studio = _dbContext.Studios.Find(id) ?? throw new Exception("Studio not found");

    _dbContext.Remove(studio);

    return _dbContext.SaveChanges();
  }

  public Task<Studio?> GetAsync(Guid id)
  {
    return _dbContext.Studios.FindAsync(id).AsTask();
  }

  public IEnumerable<Studio> GetStudioPages(PaginationReq req)
  {
    return _dbContext.Studios
            .Where(stu => stu.Status == StudioStatusEnum.Active)
            .Include(stu => stu.WorkingTimes)
            .Include(stu => stu.ListMedia)
            .Take(req.PageSize)
            .Skip(req.Page)
            .ToList();
  }

  public IEnumerable<Studio> GetStudios()
  {
    return _dbContext.Studios.ToList();
  }

  public int Update(Studio studio)
  {
    _dbContext.Studios.Update(studio);
    return _dbContext.SaveChanges();
  }

  public int UpdateStudioStatus(Guid id, StudioStatusEnum status)
  {
    var studio = _dbContext.Studios.Find(id) ?? throw new Exception("Studio not found");
    studio.Status = status;
    _dbContext.Studios.Update(studio);
    return _dbContext.SaveChanges();
  }
}
