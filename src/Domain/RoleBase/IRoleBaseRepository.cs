namespace art_tattoo_be.Domain.RoleBase;

interface IRoleBaseRepository
{
  // Role
  IEnumerable<Role> GetRoles();
  Role? GetRoleById(int id);

  int CreateRole(Role role);

  int UpdateRolePermission(int id, IEnumerable<string> permissionIds);

  int DeleteRole(int id);

  // Permission
  IEnumerable<Permission> GetPermissions();
  Permission? GetPermissionById(string slug);
  int CreatePermission(Permission permission);

  int UpdatePermission(Permission permission);

  int DeletePermission(string slug);
}