namespace art_tattoo_be.Infrastructure.Repositories;

using System;
using art_tattoo_be.Domain.RoleBase;
using art_tattoo_be.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class RoleBaseRepository : IRoleBaseRepository
{
  private readonly ArtTattooDbContext _dbContext;

  public RoleBaseRepository(ArtTattooDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<IEnumerable<Role>> GetRolesAsync()
  {
    return await _dbContext.Roles.ToListAsync();
  }

  public async Task<Role> GetRoleByIdAsync(Guid id)
  {
    return await _dbContext.Roles
        .FirstOrDefaultAsync(r => r.Id == id);
  }

  public int CreateRole(Role role)
  {
    _dbContext.Roles.Add(role);
    return _dbContext.SaveChanges();
  }

  public async Task<Role> DeleteRole(Guid id)
  {
    var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == id);
    if (role == null)
    {
      return null;
    }

    _dbContext.Roles.Remove(role);
    await _dbContext.SaveChangesAsync();
    return role;
  }

  public async Task<IEnumerable<Permission>> GetPermissionsAsync()
  {
    return await _dbContext.Permissions.ToListAsync();
  }

  public IEnumerable<Permission> GetPermissions()
  {
    return _dbContext.Permissions.ToList();
  }

  public async Task<Permission> GetPermissionByIdAsync(Guid id)
  {
    return await _dbContext.Permissions.FirstOrDefaultAsync(p => p.Id == id);
  }

  public int CreatePermission(Permission permission)
  {
    _dbContext.Permissions.Add(permission);
    return _dbContext.SaveChanges();
  }

  public async Task<Permission> DeletePermission(Guid id)
  {
    var permission = await _dbContext.Permissions.FirstOrDefaultAsync(p => p.Id == id);
    if (permission == null)
    {
      return null;
    }

    _dbContext.Permissions.Remove(permission);
    await _dbContext.SaveChangesAsync();
    return permission;
  }
}
