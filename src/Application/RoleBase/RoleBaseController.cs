namespace art_tattoo_be.Application.RoleBase;
using Microsoft.AspNetCore.Mvc;
using art_tattoo_be.Infrastructure.Repositories;
using art_tattoo_be.Domain.RoleBase;
using art_tattoo_be.Infrastructure.Database;
using art_tattoo_be.Application.Shared;
using AutoMapper;

[Produces("application/json")]
[ApiController]
[Route("api/v1")]
public class RoleBaseController : ControllerBase
{
  private readonly ILogger<RoleBaseController> _logger;
  private readonly IRoleBaseRepository _roleBaseRepo;
  private readonly IMapper _mapper;

  public RoleBaseController(ILogger<RoleBaseController> logger, ArtTattooDbContext dbContext, IMapper mapper)
  {
    _logger = logger;
    _roleBaseRepo = new RoleBaseRepository(dbContext);
    _mapper = mapper;
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

    try
    {
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
    catch (Exception)
    {
      return BadRequest(new BaseResp
      {
        Message = "Slug already exists!"
      });
    }
  }

  [HttpPut("permission/{slug}")]
  public IActionResult UpdatePermission([FromRoute] string slug, [FromBody] UpdatePermissionReq req)
  {
    _logger.LogInformation("UpdatePermission");

    // check if permission exists
    var p = _roleBaseRepo.GetPermissionById(slug);
    if (p == null)
    {
      return NotFound(new BaseResp
      {
        Message = "Permission not found!"
      });
    }
    // update the field if not null
    if (req.Name != null)
    {
      p.Name = req.Name;
    }

    if (req.Description != null)
    {
      p.Description = req.Description;
    }

    var result = _roleBaseRepo.UpdatePermission(p);

    if (result > 0)
    {
      return Ok(new BaseResp
      {
        Message = "Update permission successfully!"
      });
    }
    else
    {
      return BadRequest();
    }
  }

  [HttpDelete("permission/{slug}")]
  public IActionResult DeletePermission([FromRoute] string slug)
  {
    _logger.LogInformation("DeletePermission");

    var result = _roleBaseRepo.DeletePermission(slug);

    if (result > 0)
    {
      return Ok(new BaseResp
      {
        Message = "Delete permission successfully!"
      });
    }
    else
    {
      return BadRequest();
    }
  }

  [HttpGet("role")]
  public IActionResult GetRole()
  {
    _logger.LogInformation("GetRole");

    var roles = _roleBaseRepo.GetRoles();

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

  [HttpGet("role/{id}")]
  public IActionResult GetRoleById([FromRoute] int id)
  {
    _logger.LogInformation("GetRoleById");

    var role = _roleBaseRepo.GetRoleById(id);

    if (role == null)
    {
      return NotFound(new BaseResp
      {
        Message = "Role not found!"
      });
    }

    return Ok(role);
  }

  [HttpPut("role/{id}")]
  public IActionResult UpdateRole([FromRoute] int id, [FromBody] UpdateRoleReq req)
  {
    _logger.LogInformation("UpdateRole");

    // check if role exists
    var role = _roleBaseRepo.GetRoleById(id);
    if (role == null)
    {
      return NotFound(new BaseResp
      {
        Message = "Role not found!"
      });
    }

    // update the field if not null
    var result = _roleBaseRepo.UpdateRolePermission(id, req.PermissionIds);

    if (result > 0)
    {
      return Ok(new BaseResp
      {
        Message = "Update role successfully!"
      });
    }
    else
    {
      return BadRequest(new BaseResp
      {
        Message = "Update role failed!",
      });
    }
  }

  [HttpDelete("role/{id}")]
  public IActionResult DeleteRole([FromRoute] int id)
  {
    _logger.LogInformation("DeleteRole");

    var result = _roleBaseRepo.DeleteRole(id);

    if (result > 0)
    {
      return Ok(new BaseResp
      {
        Message = "Delete role successfully!"
      });
    }
    else
    {
      return BadRequest();
    }
  }
}