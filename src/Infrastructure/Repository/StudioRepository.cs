namespace art_tattoo_be.Infrastructure.Repository;
using art_tattoo_be.Application.DTOs.Studio;
using art_tattoo_be.Application.Shared.Constant;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Media;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

public class StudioRepository : IStudioRepository
{
  private readonly ArtTattooDbContext _dbContext;

  public StudioRepository(ArtTattooDbContext dbContext)
  {
    _dbContext = dbContext;
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

  public async Task<int> CreateStudioUserAsync(StudioUser studioUser, int roleId)
  {
    var user = await _dbContext.Users.FindAsync(studioUser.UserId);

    if (user == null)
    {
      return 0;
    }
    user.RoleId = roleId;

    await _dbContext.StudioUsers.AddAsync(studioUser);

    return await _dbContext.SaveChangesAsync();
  }

  public int DeleteStudio(Guid id)
  {
    var studio = _dbContext.Studios.Find(id) ?? throw new Exception("Studio not found");

    _dbContext.Remove(studio);

    return _dbContext.SaveChanges();
  }

  public int DeleteStudioUser(Guid id)
  {
    var studioUser = _dbContext.StudioUsers.FirstOrDefault(s => s.UserId == id) ?? throw new Exception("Studio user not found");

    studioUser.IsDisabled = true;
    studioUser.UserId = Guid.Parse(UserConst.USER_DELETED);

    return _dbContext.SaveChanges();
  }

  public Task<Studio?> GetAsync(Guid id)
  {
    return _dbContext.Studios
    .Include(stu => stu.ListMedia)
    .Include(stu => stu.WorkingTimes)
    .FirstOrDefaultAsync(stu => stu.Id == id);
  }

  public IEnumerable<StudioUser> GetStudioArtist(Guid studioId)
  {
    return _dbContext.StudioUsers
    .Include(user => user.User)
    .Where(user => user.StudioId == studioId)
    .Where(user => user.User.RoleId == RoleConst.ARTIST_ID)
    .Where(user => user.UserId != Guid.Parse(UserConst.USER_DELETED))
    .Where(user => user.IsDisabled == false)
    .OrderByDescending(user => user.CreatedAt)
    .ToList();
  }

  public Guid GetStudioIdByUserId(Guid userId)
  {
    var studioUser = _dbContext.StudioUsers.FirstOrDefault(s => s.UserId == userId);
    return studioUser != null ? studioUser.StudioId : Guid.Empty;
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

  public StudioUser? GetStudioUser(Guid id)
  {
    return _dbContext.StudioUsers
    .Include(stu => stu.User)
    .FirstOrDefault(stu => stu.Id == id);
  }

  public Guid GetStudioUserIdByUserId(Guid userId)
  {
    var studioUser = _dbContext.StudioUsers.FirstOrDefault(s => s.UserId == userId);
    return studioUser != null ? studioUser.Id : Guid.Empty;
  }

  public StudioUserList GetStudioUsers(GetStudioUserQuery req)
  {
    string searchKeyword = req.SearchKeyword ?? "";

    var query = _dbContext.StudioUsers
    .Include(user => user.User)
    .Where(user => user.StudioId == req.StudioId)
    .Where(user => user.UserId != Guid.Parse(UserConst.USER_DELETED))
    .Where(user => user.User.FullName.Contains(searchKeyword) || user.User.Email.Contains(searchKeyword));

    int totalCount = query.Count();

    var studioUsers = query
    .Select(user => new StudioUser
    {
      Id = user.Id,
      StudioId = user.StudioId,
      UserId = user.UserId,
      IsDisabled = user.IsDisabled,
      CreatedAt = user.CreatedAt,
      UpdatedAt = user.UpdatedAt,
      User = user.User,
    })
    .OrderByDescending(user => user.CreatedAt)
    .Skip(req.Page * req.PageSize)
    .Take(req.PageSize)
    .ToList();

    return new StudioUserList
    {
      Users = studioUsers,
      TotalCount = totalCount
    };
  }

  public IEnumerable<StudioWorkingTime> GetStudioWorkingTime(Guid studioId)
  {
    return _dbContext.StudioWorkingTimes.Where(w => w.StudioId == studioId).ToList();
  }

  public bool IsExist(Guid id)
  {
    return _dbContext.Studios.Any(stu => stu.Id == id);
  }

  public bool IsStudioUserExist(Guid userId)
  {
    return _dbContext.StudioUsers.Any(stu => stu.UserId == userId);
  }

  public bool IsStudioUserExist(Guid userId, Guid studioId)
  {
    return _dbContext.StudioUsers.Any(stu => stu.UserId == userId && stu.StudioId == studioId);
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

  public int UpdateStudioUser(Guid id, UpdateStudioUserReq req)
  {
    var studioUser = _dbContext.StudioUsers.Include(u => u.User).FirstOrDefault(s => s.Id == id) ?? throw new Exception("Studio user not found");
    studioUser.IsDisabled = req.IsDisabled;

    if (req != null && req.RoleId != null && studioUser.User.RoleId > RoleConst.SYSTEM_STAFF_ID)
    {
      studioUser.User.RoleId = req.RoleId.Value;
    }
    else
    {
      throw new Exception("Not permission to update role");
    }

    _dbContext.StudioUsers.Update(studioUser);
    return _dbContext.SaveChanges();
  }
}
