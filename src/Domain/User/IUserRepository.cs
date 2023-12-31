namespace art_tattoo_be.Domain.User;

using art_tattoo_be.Application.DTOs.Analytics;
using art_tattoo_be.Application.DTOs.User;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Media;


public interface IUserRepository
{
  UserList GetUsers(GetUserQuery req);
  UserList SearchUsers(GetUserQuery req);
  UserAdminDashboard GetUserAdminDashboard();
  User? GetUserById(Guid id);
  Task<User?> GetUserByIdAsync(Guid id);
  User? GetUserByEmail(string email);
  Task<User?> GetUserByEmailAsync(string email);
  int CreateUser(User user);
  int UpdateUser(User user);
  int UpdateUser(User user, IEnumerable<Media> mediaList);
  int UpdateUserStatus(Guid id, UserStatusEnum status);
  int UpdateUserPassword(Guid id, string password);
  int DeleteUser(Guid id);
}
