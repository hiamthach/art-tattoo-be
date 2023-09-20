namespace art_tattoo_be.Infrastructure.Repositories;

using System;
using art_tattoo_be.Domain.RoleBase;
using art_tattoo_be.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

public class RoleBaseRepository : IRoleBaseRepository
{
  private readonly ArtTattooDbContext _dbContext;

  public RoleBaseRepository(ArtTattooDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public IEnumerable<Role> GetRoles()
  {
    return _dbContext.Roles.ToList();
  }

  public RoleDto GetRoleById(Guid id)
  {
    var role = _dbContext.Roles
      .Include(r => r.RolePermission)
      .ThenInclude(rp => rp.Permission)
      .FirstOrDefault(r => r.Id == id);

    if (role == null)
    {
      return null;
    }

    var roleDto = new RoleDto
    {
      Id = role.Id,
      Name = role.Name,
      Permissions = role.RolePermission.Select(rp => rp.Permission).Select(p =>
      {
        return new Permission
        {
          Id = p.Id,
          Name = p.Name,
          Slug = p.Slug,
          Description = p.Description,
        };
      }).ToList(),
    };

    return roleDto;
  }

  public int CreateRole(Role role)
  {
    _dbContext.Roles.Add(role);
    return _dbContext.SaveChanges();
  }

  public int DeleteRole(Guid id)
  {
    var role = _dbContext.Roles.FirstOrDefault(r => r.Id == id);
    if (role == null)
    {
      return 0;
    }

    var result = _dbContext.Roles.Remove(role);
    return _dbContext.SaveChanges();
  }

  public int UpdateRolePermission(Guid roleId, IEnumerable<Guid> permissionIds)
  {
    // Get the existing permissions associated with the role.
    var existingPermissions = _dbContext.RolePermissions
        .Where(rp => rp.RoleId == roleId)
        .ToList();

    // Remove permissions that are no longer in the list.
    foreach (var existingPermission in existingPermissions)
    {
      if (!permissionIds.Contains(existingPermission.PermissionId))
      {
        _dbContext.RolePermissions.Remove(existingPermission);
      }
    }

    // Add new permissions.
    foreach (var permissionId in permissionIds)
    {
      if (!existingPermissions.Any(rp => rp.PermissionId == permissionId))
      {
        var rolePermission = new RolePermission
        {
          RoleId = roleId,
          PermissionId = permissionId,
        };

        _dbContext.RolePermissions.Add(rolePermission);
      }
    }

    // Save changes to the database.
    return _dbContext.SaveChanges();
  }

  public IEnumerable<Permission> GetPermissions()
  {
    return _dbContext.Permissions.ToList();
  }

  public Permission GetPermissionById(Guid id)
  {
    return _dbContext.Permissions
      .FirstOrDefault(p => p.Id == id);
  }

  public int CreatePermission(Permission permission)
  {
    _dbContext.Permissions.Add(permission);
    return _dbContext.SaveChanges();
  }

  public int UpdatePermission(Permission permission)
  {
    _dbContext.Entry(permission).State = EntityState.Modified;
    return _dbContext.SaveChanges();
  }

  public int DeletePermission(Guid id)
  {
    var permission = _dbContext.Permissions.FirstOrDefault(p => p.Id == id);
    if (permission == null)
    {
      return 0;
    }

    var result = _dbContext.Permissions.Remove(permission);
    return _dbContext.SaveChanges();
  }
}
