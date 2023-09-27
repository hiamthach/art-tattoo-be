using art_tattoo_be.Application.Shared.Enum;

namespace art_tattoo_be.Domain.User;

public interface IUserRepository
{
  IEnumerable<User> GetUsers();
  User? GetUserById(Guid id);
  User? GetUserByEmail(string email);
  int CreateUser(User user);
  int UpdateUser(User user);
  int UpdateUserStatus(Guid id, UserStatusEnum status);
  int UpdateUserPassword(Guid id, string password);
  int DeleteUser(Guid id);
}
