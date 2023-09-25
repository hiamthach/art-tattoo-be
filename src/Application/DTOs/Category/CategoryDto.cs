namespace art_tattoo_be.Application.DTOs.Category;

using art_tattoo_be.Domain.Category;
using AutoMapper;

public class CategoryDto
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public string? Description { get; set; }
  public string? Image { get; set; }
}

// AutoMapper
public class CategoryProfile : Profile
{
  public CategoryProfile()
  {
    CreateMap<Category, CategoryDto>();
  }
}
