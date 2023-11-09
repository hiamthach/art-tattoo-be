using art_tattoo_be.Domain.Media;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Infrastructure.Database;
using art_tattoo_be.src.Application.DTOs.StudioService;
using art_tattoo_be.src.Domain.Studio;
using Microsoft.EntityFrameworkCore;

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

    public int DeleteStudioService(Guid id)
    {
      var studioService = _dbContext.StudioServices.Find(id) ?? throw new Exception("Studio Service not found");
      _dbContext.Remove(studioService);
      return _dbContext.SaveChanges();
    }

    public IEnumerable<StudioService> GetAll()
    {
      return _dbContext.StudioServices.ToList();
    }

    public StudioService GetById(Guid id)
    {
      return _dbContext.StudioServices
      .Include(stuSer => stuSer.Category)
      .Include(stuSer => stuSer.ListMedia)
      .FirstOrDefault(stuSer => stuSer.Id == id) ?? throw new Exception("Studio Service not found");
    }

    public StudioServiceList GetStudioServicePages(GetStudioServiceReq req)
    {
      string searchKeyword = req.SearchKeyword ?? "";
      var query = _dbContext.StudioServices
      .Where(stuSer => stuSer.Name.Contains(searchKeyword))
      .Where(stuSer => stuSer.StudioId == req.StudioId)
      .Where(stuSer => req.IsStudio || !stuSer.IsDisabled);

      int totalCount = query.Count();

      var studioService = query
      .Include(stuSer => stuSer.Category)
      .Include(stuSer => stuSer.ListMedia)
      .Select(stuSer => new StudioService
      {
        Id = stuSer.Id,
        StudioId = stuSer.StudioId,
        CategoryId = stuSer.CategoryId,
        Name = stuSer.Name,
        Description = stuSer.Description,
        MinPrice = stuSer.MinPrice,
        MaxPrice = stuSer.MaxPrice,
        Discount = stuSer.Discount,
        Category = stuSer.Category,
        ExpectDuration = stuSer.ExpectDuration,
        Thumbnail = stuSer.Thumbnail,
        IsDisabled = stuSer.IsDisabled,
        ListMedia = stuSer.ListMedia
      })

      .OrderByDescending(stuSer => stuSer.Name)
      .Skip(req.Page * req.PageSize)
      .Take(req.PageSize)
      .ToList();
      return new StudioServiceList
      {
        StudioServices = studioService,
        TotalCount = totalCount
      };
    }

    public int UpdateStudioService(StudioService studioService, IEnumerable<Media> mediaList)
    {
      var removeMedia = studioService.ListMedia.Where(m => !mediaList.Select(m => m.Id).Contains(m.Id)).ToList();
      var newMedia = mediaList.Where(m => !studioService.ListMedia.Select(m => m.Id).Contains(m.Id)).ToList();

      if (removeMedia.Count > 0)
      {
        _dbContext.Medias.RemoveRange(removeMedia);
        studioService.ListMedia.RemoveAll(m => removeMedia.Select(m => m.Id).Contains(m.Id));
      }

      _dbContext.Medias.AddRange(newMedia);
      studioService.ListMedia.AddRange(newMedia);

      _dbContext.StudioServices.Update(studioService);
      return _dbContext.SaveChanges();
    }
  }
}
