namespace art_tattoo_be.Infrastructure.Repository;

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
    return _dbContext.Roles.Include(r => r.Permissions).ToList();
  }

  public Role? GetRoleById(int id)
  {
    return _dbContext.Roles.Include(r => r.Permissions).FirstOrDefault(r => r.Id == id);
  }

  public int CreateRole(Role role)
  {
    _dbContext.Roles.Add(role);
    return _dbContext.SaveChanges();
  }

  public int UpdateRolePermission(int id, IEnumerable<string> permissionSlugs)
  {
    // check if role exists
    var role = _dbContext.Roles.FirstOrDefault(r => r.Id == id) ?? throw new Exception("Role not found");

    var slugs = role.Permissions.Select(p => p.Slug).ToList();
    var removeSlugs = slugs.Except(permissionSlugs).ToList();
    var addSlugs = permissionSlugs.Except(slugs).ToList();

    role.Permissions.RemoveAll(p => removeSlugs.Contains(p.Slug));
    var newPermissions = _dbContext.Permissions.Where(p => addSlugs.Contains(p.Slug)).ToList();

    role.Permissions.AddRange(newPermissions);

    return _dbContext.SaveChanges();
  }

  public int DeleteRole(int id)
  {
    var role = _dbContext.Roles.Find(id);
    if (role == null)
    {
      throw new Exception("Role not found");
    }

    _dbContext.Roles.Remove(role);
    return _dbContext.SaveChanges();
  }

  public IEnumerable<Permission> GetPermissions()
  {
    return _dbContext.Permissions.ToList();
  }

  public int CreatePermission(Permission permission)
  {
    _dbContext.Permissions.Add(permission);

    return _dbContext.SaveChanges();
  }

  public int UpdatePermission(Permission permission)
  {
    _dbContext.Permissions.Update(permission);
    return _dbContext.SaveChanges();
  }

  public Permission? GetPermissionById(string slug)
  {
    return _dbContext.Permissions.Find(slug);
  }

  public int DeletePermission(string slug)
  {
    var permission = _dbContext.Permissions.Find(slug) ?? throw new Exception("Permission not found");

    _dbContext.Permissions.Remove(permission);
    return _dbContext.SaveChanges();
  }

  public IEnumerable<string> GetRolePermissionSlugs(int id)
  {
    var role = _dbContext.Roles.Include(r => r.Permissions).FirstOrDefault(r => r.Id == id);
    if (role == null)
    {
      return new List<string>();
    }

    return role.Permissions.Select(p => p.Slug).ToList();
  }
}
