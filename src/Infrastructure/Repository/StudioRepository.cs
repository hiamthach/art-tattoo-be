using art_tattoo_be.Application.DTOs.Studio;
using art_tattoo_be.Application.Shared.Constant;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Media;
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
    return _dbContext.Studios
    .Include(stu => stu.ListMedia)
    .Include(stu => stu.WorkingTimes)
    .FirstOrDefaultAsync(stu => stu.Id == id);
  }

  public StudioList GetStudioPages(GetStudioQuery req)
  {
    // Init maximum north, east, south, west
    double north = Coordinates.MAX_NORTH;
    double east = Coordinates.MAX_EAST;
    double south = Coordinates.MAX_SOUTH;
    double west = Coordinates.MAX_WEST;

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
    .Where(stu => stu.Latitude <= north && stu.Latitude >= south && stu.Longitude <= east && stu.Longitude >= west)
    .Where(stu => stu.Name.Contains(searchKeyword));

    // var query = _dbContext.Studios
    //  .FromSqlRaw("SELECT * FROM Studios WHERE Latitude <= {0} AND Latitude >= {1} AND Longitude <= {2} AND Longitude >= {3} AND Name COLLATE SQL_Latin1_General_CP1_CI_AI LIKE {4}", north, south, east, west, $"%{unidecodedKeyword}%");

    int totalCount = query.Count();

    var studios = query
        .Include(stu => stu.WorkingTimes)
        .Include(stu => stu.ListMedia)
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
          WorkingTimes = stu.WorkingTimes,
          ListMedia = stu.ListMedia,
        })
        .OrderByDescending(stu => stu.Name)
        .Skip(req.Page * req.PageSize)
        .Take(req.PageSize)
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

  public int Update(Studio studio, IEnumerable<Media> mediaList)
  {

    // Check if the StudioWorkingTimes are being tracked by EF Core.
    if (studio.WorkingTimes != null)
    {
      // Remove the existing StudioWorkingTimes and add the new ones.
      // filter out working times that has empty Guid

      var workingTimes = studio.WorkingTimes.Where(w => w.Id == Guid.Empty).ToList();

      if (workingTimes.Count > 0)
      {
        _dbContext.StudioWorkingTimes.RemoveRange(_dbContext.StudioWorkingTimes.Where(w => w.StudioId == studio.Id));
        _dbContext.StudioWorkingTimes.AddRange(studio.WorkingTimes.Select(w =>
        {
          w.Id = Guid.NewGuid();
          return w;
        }));
      }
    }

    // clear old media
    var removeMedia = studio.ListMedia.Where(m => !mediaList.Select(m => m.Id).Contains(m.Id)).ToList();
    var newMedia = mediaList.Where(m => !studio.ListMedia.Select(m => m.Id).Contains(m.Id)).ToList();

    if (removeMedia.Count > 0)
    {
      _dbContext.Medias.RemoveRange(removeMedia);
      studio.ListMedia.RemoveAll(m => removeMedia.Select(m => m.Id).Contains(m.Id));
    }

    _dbContext.Medias.AddRange(newMedia);
    studio.ListMedia.AddRange(newMedia);

    // Update the Studio entity.
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
