namespace art_tattoo_be.Application.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using art_tattoo_be.Application.DTOs.RoleBase;
using art_tattoo_be.Domain.RoleBase;
using art_tattoo_be.Application.Shared;
using art_tattoo_be.Application.Shared.Handler;
using art_tattoo_be.Application.Middlewares;
using art_tattoo_be.Infrastructure.Cache;

[Produces("application/json")]
[ApiController]
[Route("api")]
public class RoleBaseController : ControllerBase
{
  private readonly ILogger<RoleBaseController> _logger;
  private readonly IRoleBaseRepository _roleBaseRepo;
  private readonly ICacheService _cacheService;
  private readonly IMapper _mapper;

  public RoleBaseController(ILogger<RoleBaseController> logger, IMapper mapper, IRoleBaseRepository roleBaseRepository, ICacheService cacheService)
  {
    _logger = logger;
    _roleBaseRepo = roleBaseRepository;
    _mapper = mapper;
    _cacheService = cacheService;
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_PERMISSION)]
  [HttpGet("permission")]
  public async Task<IActionResult> GetPermission()
  {
    _logger.LogInformation("GetPermission");
    try
    {
      var redisKey = "permissions";

      var permissionsCache = await _cacheService.Get<List<PermissionDto>>(redisKey);
      if (permissionsCache != null)
      {
        return Ok(permissionsCache);
      }

      var permissions = _roleBaseRepo.GetPermissions();

      var permissionsMapped = _mapper.Map<List<PermissionDto>>(permissions);

      await _cacheService.Set(redisKey, permissionsMapped, TimeSpan.FromDays(1));

      return Ok(permissionsMapped);
    }
    catch (Exception e)
    {
      _logger.LogError("GetPermission: {@e}", e);
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_PERMISSION)]
  [HttpPost("permission")]
  public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionReq req)
  {
    _logger.LogInformation("CreatePermission: {@req}", req);
    try
    {
      var p = new Permission
      {
        Name = req.Name,
        Slug = req.Slug,
        Description = req.Description,
      };
      var result = _roleBaseRepo.CreatePermission(p);

      var redisKey = "permissions";

      await _cacheService.Remove(redisKey);
      if (result > 0)
      {
        return Ok(new CreatePermissionResp
        {
          Message = "Create permission successfully!",
          Permission = _mapper.Map<PermissionDto>(p)
        });
      }
      else
      {
        return ErrorResp.SomethingWrong("Create permission failed!");
      }
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_PERMISSION)]
  [HttpPut("permission/{slug}")]
  public async Task<IActionResult> UpdatePermission([FromRoute] string slug, [FromBody] UpdatePermissionReq req)
  {
    _logger.LogInformation("UpdatePermission");

    // check if permission exists
    try
    {
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

      var redisKey = "permissions";

      await _cacheService.Remove(redisKey);
      if (result > 0)
      {
        return Ok(new BaseResp
        {
          Message = "Update permission successfully!",
          Success = true
        });
      }
      else
      {
        return ErrorResp.SomethingWrong("Update permission failed!");
      }
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_PERMISSION)]
  [HttpDelete("permission/{slug}")]
  public async Task<IActionResult> DeletePermission([FromRoute] string slug)
  {
    _logger.LogInformation("DeletePermission");

    try
    {
      var result = _roleBaseRepo.DeletePermission(slug);

      var redisKey = "permissions";

      await _cacheService.Remove(redisKey);

      if (result > 0)
      {
        return Ok(new BaseResp
        {
          Message = "Delete permission successfully!",
          Success = true
        });
      }
      else
      {
        return ErrorResp.SomethingWrong("Delete permission failed!");
      }
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_ROLE)]
  [HttpGet("role")]
  public async Task<IActionResult> GetRole()
  {
    _logger.LogInformation("GetRole");

    try
    {
      var redisKey = "roles";

      var rolesCache = await _cacheService.Get<List<RoleDto>>(redisKey);
      if (rolesCache != null)
      {
        return Ok(rolesCache);
      }

      var roles = _roleBaseRepo.GetRoles();

      var rolesMapped = _mapper.Map<List<RoleDto>>(roles);

      await _cacheService.Set(redisKey, rolesMapped, TimeSpan.FromDays(1));

      return Ok(rolesMapped);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_ROLE)]
  [HttpPost("role")]
  public async Task<IActionResult> CreateRole([FromBody] CreateRoleReq req)
  {
    _logger.LogInformation("CreateRole");

    try
    {
      var result = _roleBaseRepo.CreateRole(new Role
      {
        Name = req.Name,
      });

      var redisKey = "roles";

      await _cacheService.ClearWithPattern(redisKey);

      if (result > 0)
      {
        return Ok(new BaseResp
        {
          Message = "Create role successfully!",
          Success = true
        });
      }
      else
      {
        return ErrorResp.SomethingWrong("Create role failed!");
      }
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_ROLE)]
  [HttpGet("role/{id}")]
  public async Task<IActionResult> GetRoleById([FromRoute] int id)
  {
    _logger.LogInformation("GetRoleById");

    try
    {
      var redisKey = $"roles:{id}";
      var roleCache = await _cacheService.Get<RoleDto>(redisKey);
      if (roleCache != null)
      {
        return Ok(roleCache);
      }

      var role = _roleBaseRepo.GetRoleById(id);
      var roleMapped = _mapper.Map<RoleDto>(role);

      await _cacheService.Set(redisKey, roleMapped, TimeSpan.FromDays(1));
      if (role == null)
      {
        return NotFound(new BaseResp
        {
          Message = "Role not found!"
        });
      }

      return Ok(roleMapped);
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_ROLE)]
  [HttpPut("role/{id}")]
  public async Task<IActionResult> UpdateRole([FromRoute] int id, [FromBody] UpdateRoleReq req)
  {
    _logger.LogInformation("UpdateRole");

    // check if role exists
    try
    {
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

      var redisKey = "roles";

      await _cacheService.ClearWithPattern(redisKey);

      if (result > 0)
      {
        return Ok(new BaseResp
        {
          Message = "Update role successfully!",
          Success = true
        });
      }
      else
      {
        return ErrorResp.SomethingWrong("Update role failed!");
      }
    }
    catch (Exception e)
    {
      return ErrorResp.SomethingWrong(e.Message);
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_ROLE)]
  [HttpDelete("role/{id}")]
  public async Task<IActionResult> DeleteRole([FromRoute] int id)
  {
    _logger.LogInformation("DeleteRole");

    var result = _roleBaseRepo.DeleteRole(id);

    var redisKey = "roles";

    await _cacheService.ClearWithPattern(redisKey);

    if (result > 0)
    {
      return Ok(new BaseResp
      {
        Message = "Delete role successfully!",
        Success = true
      });
    }
    else
    {
      return ErrorResp.SomethingWrong("Delete role failed!");
    }
  }

  [Protected]
  [Permission(PermissionSlugConst.MANAGE_OWNED_STUDIO, PermissionSlugConst.MANAGE_STUDIO)]
  [HttpGet("role-base/test")]
  public IActionResult Test()
  {
    _logger.LogInformation("Test");

    var permissions = HttpContext.Items["permission"];

    return Ok(new
    {
      Permission = permissions,
      Message = "Success"
    });
  }
}
