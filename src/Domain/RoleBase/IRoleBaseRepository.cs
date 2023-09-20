using Microsoft.AspNetCore.Mvc;

namespace art_tattoo_be.Domain.RoleBase;

interface IRoleBaseRepository
{
  // Role
  Task<Role> GetRoleByIdAsync(Guid id);
  int CreateRole(Role role);
  Task<Role> DeleteRole(Guid id);

  // Permission
  Task<Permission> GetPermissionByIdAsync(Guid id);
  int CreatePermission(Permission permission);
  Task<Permission> DeletePermission(Guid id);
}