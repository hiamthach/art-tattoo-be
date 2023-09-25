namespace art_tattoo_be.Domain.Category;

interface ICategoryRepository
{
  IEnumerable<Category> GetAll();
  Category GetById(int id);
  int CreateCategory(Category category);
  int Delete(int id);
}
