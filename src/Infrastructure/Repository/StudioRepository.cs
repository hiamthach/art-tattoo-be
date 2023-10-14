using art_tattoo_be.Application.DTOs.Pagination;
using art_tattoo_be.Application.DTOs.Studio;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Infrastructure.Database;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace art_tattoo_be.Infrastructure.Repository;

public class StudioRepository : IStudioRepository
{
  private readonly ArtTattooDbContext _dbContext;
  private readonly IMapper _mapper;

  public StudioRepository(IMapper mapper, ArtTattooDbContext dbContext)
  {
    _dbContext = dbContext;
    _mapper = mapper;
  }

  public int Count()
  {
    return _dbContext.Studios
    // .Where(stu => stu.Status == StudioStatusEnum.Active)
    .Count();
  }

  public async Task<int> CreateAsync(Studio studio)
  {
    await _dbContext.Studios.AddAsync(studio);
    await _dbContext.StudioWorkingTimes.AddRangeAsync(studio.WorkingTimes);
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

  public StudioList GetStudioPages(GetStudioQuery req)
  {
    // Init maximum north, east, south, west
    double north = 90;
    double east = 180;
    double south = -90;
    double west = -180;

    string searchKeyword = req.SearchKeyword ?? "";

    // check view exist
    if (req.ViewPortNE != null && req.ViewPortSW != null)
    {
      north = req.ViewPortNE.Lat;
      east = req.ViewPortNE.Lng;
      south = req.ViewPortSW.Lat;
      west = req.ViewPortSW.Lng;
    }

    var query = _dbContext.Studios
    // .Where(stu => stu.Status == StudioStatusEnum.Active)
    .Include(stu => stu.WorkingTimes)
    .Include(stu => stu.ListMedia)
    .Where(stu => stu.Latitude <= north && stu.Latitude >= south && stu.Longitude <= east && stu.Longitude >= west)
    .Where(stu => stu.Name.Contains(searchKeyword));

    int totalCount = query.Count();

    var studios = query
        .Select(stu => new Studio
        {
          Id = stu.Id,
          Name = stu.Name,
          Slogan = stu.Slogan,
          Introduction = stu.Introduction,
          Logo = stu.Logo,
          Phone = stu.Phone,
          Email = stu.Email,
          Website = stu.Website,
          Facebook = stu.Facebook,
          Instagram = stu.Instagram,
          Address = stu.Address,
          Latitude = stu.Latitude,
          Longitude = stu.Longitude,
        })
        .OrderByDescending(stu => stu.Name)
        .Take(req.PageSize)
        .Skip(req.Page)
        .ToList();

    return new StudioList
    {
      Studios = studios,
      TotalCount = totalCount
    };
  }

  public IEnumerable<Studio> GetStudios()
  {
    return _dbContext.Studios.ToList();
  }

  public int Update(Studio studio)
  {

    // Check if the StudioWorkingTimes are being tracked by EF Core.
    if (studio.WorkingTimes != null)
    {
      // Remove the existing StudioWorkingTimes and add the new ones.
      _dbContext.StudioWorkingTimes.RemoveRange(_dbContext.StudioWorkingTimes.Where(w => w.StudioId == studio.Id));
      _dbContext.StudioWorkingTimes.AddRange(studio.WorkingTimes);
    }

    // Update the Studio entity.
    _dbContext.Update(studio);

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
