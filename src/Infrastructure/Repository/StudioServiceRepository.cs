using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using art_tattoo_be.Domain.Category;
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
      studioService.Id = Guid.NewGuid();
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
      var studioService = _dbContext.StudioServices.Find(id) ?? throw new Exception("Studio Service not found");
      studioService.Category = _dbContext.Categories.Find(studioService.CategoryId) ?? throw new Exception("Category not found");
      return studioService;
    }

    public StudioServiceList GetStudioServicePages(GetStudioServiceQuery req)
    {
      string searchKeyword = req.SearchKeyword ?? "";
      var query = _dbContext.StudioServices
      .Where(stuser => stuser.Name.Contains(searchKeyword));
      int totalCount = query.Count();

      var studioService = query
      .Include(stuser => stuser.Category)
      .Select(stuser => new StudioService
      {
        Id = stuser.Id,
        StudioId = stuser.StudioId,
        CategoryId = stuser.CategoryId,
        Name = stuser.Name,
        Description = stuser.Description,
        MinPrice = stuser.MinPrice,
        MaxPrice = stuser.MaxPrice,
        Discount = stuser.Discount,
        Category = stuser.Category
      })
      .OrderByDescending(stuser => stuser.Name)
      .Skip(req.Page * req.PageSize)
      .Take(req.PageSize)
      .ToList();
      return new StudioServiceList
      {
        StudioServices = studioService,
        TotalCount = totalCount
      };
    }
  }
}