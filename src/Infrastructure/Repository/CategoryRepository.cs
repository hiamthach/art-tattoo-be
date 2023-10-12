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
  private readonly IMapper _mapper;

  public CategoryRepository(ArtTattooDbContext dbContext, IMapper mapper)
  {
    _dbContext = dbContext;
    _mapper = mapper;
  }

  public IEnumerable<CategoryDto> GetAll()
  {
    List<CategoryDto> listCategoryDto = _dbContext.Categories.Select(c => _mapper.Map<CategoryDto>(c)).ToList();
    return listCategoryDto;
  }

  public CategoryDto GetById(int id)
  {
    CategoryDto categoryDto = _mapper.Map<CategoryDto>(_dbContext.Categories.Find(id)) ?? throw new Exception("Category not found");
    return categoryDto;
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