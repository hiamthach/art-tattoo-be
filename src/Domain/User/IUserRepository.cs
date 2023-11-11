using art_tattoo_be.Application.DTOs.User;
using art_tattoo_be.Application.Shared.Enum;

namespace art_tattoo_be.Domain.User;

public interface IUserRepository
{
  UserList GetUsers(GetUserQuery req);
  UserList SearchUsers(GetUserQuery req);
  User? GetUserById(Guid id);
  Task<User?> GetUserByIdAsync(Guid id);
  User? GetUserByEmail(string email);
  Task<User?> GetUserByEmailAsync(string email);
  int CreateUser(User user);
  int UpdateUser(User user);
  int UpdateUserStatus(Guid id, UserStatusEnum status);
  int UpdateUserPassword(Guid id, string password);
  int DeleteUser(Guid id);
}
