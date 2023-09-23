namespace art_tattoo_be.Application.Category;

using art_tattoo_be.Domain.Category;
using art_tattoo_be.Infrastructure.Database;
using art_tattoo_be.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;


[Produces("application/json")]
[ApiController]
[Route("api/category")]
public class CategoryController : ControllerBase
{
  private readonly ILogger<CategoryController> _logger;

  private readonly ICategoryRepository _cateRepo;

  public CategoryController(ILogger<CategoryController> logger, ArtTattooDbContext dbContext)
  {
    _logger = logger;
    _cateRepo = new CategoryRepository(dbContext);
  }

  [HttpGet]
  public IActionResult GetAll()
  {
    _logger.LogInformation("Get categories:");

    var categories = _cateRepo.GetAll();

    return Ok(categories);
  }

  [HttpGet("{id}")]
  public IActionResult GetById([FromRoute] int id)
  {
    _logger.LogInformation("Get category: @id", id);

    var category = _cateRepo.GetById(id);

    return Ok(category);
  }

  [HttpPost]
  public IActionResult CreateCategory([FromBody] CreateCategoryReq body)
  {
    _logger.LogInformation("Create category: @body", body);

    try
    {
      var result = _cateRepo.CreateCategory(new Category
      {
        Name = body.Name,
        Description = body.Description,
        Image = body.Image
      });

      if (result > 0)
      {
        return Ok("Created");
      }
      else
      {
        return BadRequest("Create failed");
      }

    }
    catch (Exception e)
    {
      _logger.LogError(e, "Error creating category");
      return new JsonResult(new { error = e.Message }) { StatusCode = 500 };
    }
  }

  [HttpDelete("{id}")]
  public IActionResult DeleteCategory([FromRoute] int id)
  {
    _logger.LogInformation("Delete category: @id", id);

    try
    {
      var result = _cateRepo.Delete(id);
      if (result > 0)
      {
        return Ok("Deleted");
      }
      else
      {
        return BadRequest("Delete failed");
      }
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Error delete category");
      return new JsonResult(new { error = e.Message }) { StatusCode = 500 };
    }

  }
}