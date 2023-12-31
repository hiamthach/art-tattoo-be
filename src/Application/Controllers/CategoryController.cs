namespace art_tattoo_be.Application.Controllers;

using Microsoft.AspNetCore.Mvc;
using art_tattoo_be.Domain.Category;
using art_tattoo_be.Infrastructure.Database;
using art_tattoo_be.Infrastructure.Repository;
using art_tattoo_be.Application.Shared.Handler;
using art_tattoo_be.Application.DTOs.Category;
using AutoMapper;
using art_tattoo_be.Application.Shared;
using art_tattoo_be.Application.Middlewares;

[Produces("application/json")]
[ApiController]
[Route("api/category")]
public class CategoryController : ControllerBase
{
  private readonly ILogger<CategoryController> _logger;

  private readonly ICategoryRepository _cateRepo;

  private readonly IMapper _mapper;

  public CategoryController(ILogger<CategoryController> logger, ArtTattooDbContext dbContext, IMapper mapper)
  {
    _logger = logger;
    _cateRepo = new CategoryRepository(dbContext);
    _mapper = mapper;
  }

  [HttpGet]
  public IActionResult GetAll()
  {
    _logger.LogInformation("Get categories:");

    var categories = _cateRepo.GetAll();

    return Ok(_mapper.Map<List<CategoryDto>>(categories));
  }

  [HttpGet("{id}")]
  public IActionResult GetById([FromRoute] int id)
  {
    try
    {
      _logger.LogInformation("Get category: @id", id);

      var category = _cateRepo.GetById(id);

      if (category == null)
      {
        return ErrorResp.NotFound("Category not found");
      }
      else
      {
        return Ok(_mapper.Map<CategoryDto>(category));
      }
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_CATEGORY)]
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
        return Ok(new BaseResp { Message = "Create successfully", Success = true });
      }
      else
      {
        return ErrorResp.BadRequest("Create failed");
      }

    }
    catch (Exception e)
    {
      _logger.LogError(e, "Error creating category");
      return ErrorResp.UnknownError(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_CATEGORY)]
  [HttpDelete("{id}")]
  public IActionResult DeleteCategory([FromRoute] int id)
  {
    _logger.LogInformation("Delete category: @id", id);

    try
    {
      var result = _cateRepo.Delete(id);
      if (result > 0)
      {
        return Ok(new BaseResp { Message = "Delete successfully", Success = true });
      }
      else
      {
        return BadRequest("Delete failed");
      }
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Error delete category");
      return ErrorResp.UnknownError(e.Message);
    }

  }
}
