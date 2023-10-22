namespace art_tattoo_be.Infrastructure.Repository;

using System.Threading.Tasks;
using art_tattoo_be.Application.DTOs.User;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.User;
using art_tattoo_be.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

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

  public Task<User?> GetUserByEmailAsync(string email)
  {
    return _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
  }

  public User? GetUserById(Guid id)
  {
    return _dbContext.Users.Find(id);
  }

  public Task<User?> GetUserByIdAsync(Guid id)
  {
    return _dbContext.Users.FindAsync(id).AsTask();
  }

  public UserList GetUsers(GetUserQuery req)
  {
    string searchKeyword = req.SearchKeyword ?? "";
    var query = _dbContext.Users
      .Where(u => u.Email.Contains(searchKeyword) || u.FullName.Contains(searchKeyword));

    int totalCount = query.Count();

    var users = query
      .OrderByDescending(user => user.CreatedAt)
      .Skip(req.Page * req.PageSize)
      .Take(req.PageSize)
      .ToList();

    return new UserList
    {
      Users = users,
      TotalCount = totalCount
    };
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
