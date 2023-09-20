namespace art_tattoo_be.Application.RoleBase;
using Microsoft.AspNetCore.Mvc;
using art_tattoo_be.Infrastructure.Repositories;
using art_tattoo_be.Domain.RoleBase;
using art_tattoo_be.Infrastructure.Database;
using System.Threading.Tasks;

[Produces("application/json")]
[ApiController]
[Route("api/role-base")]
public class RoleBaseController : ControllerBase
{
  private readonly ILogger<RoleBaseController> _logger;

  private readonly IRoleBaseRepository _roleBaseRepo;

  public RoleBaseController(ILogger<RoleBaseController> logger, ArtTattooDbContext dbContext)
  {
    _logger = logger;
    _roleBaseRepo = new RoleBaseRepository(dbContext);
  }

  [HttpGet("permission")]
  public IActionResult GetPermission()
  {
    _logger.LogInformation("GetPermission");

    var permissions = _roleBaseRepo.GetPermissions();

    return Ok(permissions);
  }

  [HttpPost("permission")]
  public IActionResult CreatePermission([FromBody] CreatePermissionReq req)
  {
    _logger.LogInformation("CreatePermission: {@req}", req);

    var p = new Permission
    {
      Name = req.Name,
      Slug = req.Slug,
      Description = req.Description,
    };

    var result = _roleBaseRepo.CreatePermission(p);

    if (result > 0)
    {
      return Ok(new CreatePermissionResp
      {
        Message = "Create permission successfully!"
      });
    }
    else
    {
      return BadRequest();
    }
  }

  [HttpPut("permission/{id}")]
  public IActionResult UpdatePermission([FromRoute] Guid id, [FromBody] UpdatePermissionReq req)
  {
    _logger.LogInformation("UpdatePermission");

    return Ok(req);
  }

  [HttpDelete("permission/{id}")]
  public IActionResult DeletePermission([FromRoute] Guid id)
  {
    _logger.LogInformation("DeletePermission");

    return Ok(id);
  }

  [HttpGet("role")]
  public IActionResult GetRole()
  {
    _logger.LogInformation("GetRole");

    var roles = _roleBaseRepo.GetRolesAsync();

    return Ok(roles);
  }

  [HttpPost("role")]
  public IActionResult CreateRole([FromBody] CreateRoleReq req)
  {
    _logger.LogInformation("CreateRole");

    var result = _roleBaseRepo.CreateRole(new Role
    {
      Name = req.Name,
    });

    if (result > 0)
    {
      return Ok(new CreateRoleResp
      {
        Message = "Create role successfully!"
      });
    }
    else
    {
      return BadRequest();
    }
  }

  [HttpPut("role/{id}")]
  public IActionResult UpdateRole([FromRoute] Guid id, [FromBody] UpdateRoleReq req)
  {
    _logger.LogInformation("UpdateRole");

    return Ok(req);
  }

  [HttpDelete("role/{id}")]
  public IActionResult DeleteRole([FromRoute] Guid id)
  {
    _logger.LogInformation("DeleteRole");

    return Ok(id);
  }
}