using art_tattoo_be.Application.DTOs.Category;

namespace art_tattoo_be.Domain.Category;

interface ICategoryRepository
{
  IEnumerable<CategoryDto> GetAll();
  CategoryDto GetById(int id);
  int CreateCategory(Category category);
  int Delete(int id);
}
