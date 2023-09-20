namespace art_tattoo_be.Domain.RoleBase;

interface IRoleBaseRepository
{
  // Role
  IEnumerable<Role> GetRoles();
  RoleDto GetRoleById(Guid id);

  int CreateRole(Role role);

  int UpdateRolePermission(Guid id, IEnumerable<Guid> permissionIds);

  int DeleteRole(Guid id);

  // Permission
  IEnumerable<Permission> GetPermissions();
  Permission GetPermissionById(Guid id);
  int CreatePermission(Permission permission);

  int UpdatePermission(Permission permission);

  int DeletePermission(Guid id);
}