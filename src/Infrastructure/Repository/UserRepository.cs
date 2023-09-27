namespace art_tattoo_be.Infrastructure.Repository;

using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.User;
using art_tattoo_be.Infrastructure.Database;


public class UserRepository : IUserRepository
{
  private readonly ArtTattooDbContext _dbContext;

  public UserRepository(ArtTattooDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public int CreateUser(User user)
  {
    _dbContext.Users.Add(user);
    return _dbContext.SaveChanges();
  }

  public int DeleteUser(Guid id)
  {
    var user = _dbContext.Users.Find(id) ?? throw new Exception("User not found");
    _dbContext.Remove(user);
    return _dbContext.SaveChanges();
  }

  public User? GetUserByEmail(string email)
  {
    return _dbContext.Users.FirstOrDefault(u => u.Email == email);
  }

  public User? GetUserById(Guid id)
  {
    return _dbContext.Users.Find(id);
  }

  public IEnumerable<User> GetUsers()
  {
    return _dbContext.Users.ToList();
  }

  public int UpdateUser(User user)
  {
    _dbContext.Users.Update(user);
    return _dbContext.SaveChanges();
  }

  public int UpdateUserPassword(Guid id, string password)
  {
    var user = _dbContext.Users.Find(id) ?? throw new Exception("User not found");
    user.Password = password;
    _dbContext.Users.Update(user);
    return _dbContext.SaveChanges();
  }

  public int UpdateUserStatus(Guid id, UserStatusEnum status)
  {
    var user = _dbContext.Users.Find(id) ?? throw new Exception("User not found");
    user.Status = status;
    _dbContext.Users.Update(user);
    return _dbContext.SaveChanges();
  }
}
