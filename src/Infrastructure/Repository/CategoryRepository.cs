using art_tattoo_be.Domain.Category;
using art_tattoo_be.Infrastructure.Database;

namespace art_tattoo_be.Infrastructure.Repository;

public class CategoryRepository : ICategoryRepository
{
  private readonly ArtTattooDbContext _dbContext;

  public CategoryRepository(ArtTattooDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public IEnumerable<Category> GetAll()
  {
    throw new NotImplementedException();
  }

  public Category GetById(int id)
  {
    throw new NotImplementedException();
  }

  public int CreateCategory(Category category)
  {
    throw new NotImplementedException();
  }

  public int Delete(int id)
  {
    throw new NotImplementedException();
  }

};