using System.Runtime.CompilerServices;
using art_tattoo_be.Application.DTOs.Category;
using art_tattoo_be.Domain.Category;
using art_tattoo_be.Infrastructure.Database;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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
    return _dbContext.Categories.ToList();
  }

  public Category GetById(int id)
  {
    return _dbContext.Categories.Find(id) ?? throw new Exception("Category not found");
  }

  public int CreateCategory(Category category)
  {
    _dbContext.Categories.Add(category);
    return _dbContext.SaveChanges();
  }

  public int Delete(int id)
  {
    var category = _dbContext.Categories.Find(id) ?? throw new Exception("Category not found");
    _dbContext.Remove(category);
    return _dbContext.SaveChanges();
  }

};