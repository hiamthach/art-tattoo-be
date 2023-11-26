namespace art_tattoo_be.Infrastructure.Repository;

using System.Collections.Generic;
using System.Threading.Tasks;
using art_tattoo_be.Application.DTOs.Analytics;
using art_tattoo_be.Application.DTOs.User;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Media;
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

  public UserAdminDashboard GetUserAdminDashboard()
  {
    var userAdminDashboard = new UserAdminDashboard
    {
      TotalUser = _dbContext.Users.Count(),
      TotalUserThisMonth = _dbContext.Users.Count(u => u.CreatedAt.Month == DateTime.Now.Month),
      TotalUserLastMonth = _dbContext.Users.Count(u => u.CreatedAt.Month == DateTime.Now.AddMonths(-1).Month),
    };

    return userAdminDashboard;
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
    return _dbContext.Users.Include(u => u.ListMedia).FirstOrDefault(u => u.Id == id);
  }

  public Task<User?> GetUserByIdAsync(Guid id)
  {
    return _dbContext.Users.Include(u => u.ListMedia).FirstOrDefaultAsync(u => u.Id == id);
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

  public UserList SearchUsers(GetUserQuery req)
  {
    string searchKeyword = req.SearchKeyword ?? "";
    var query = _dbContext.Users
      .Select(u => new User
      {
        Id = u.Id,
        Email = u.Email,
        FullName = u.FullName,
        Phone = u.Phone,
        Avatar = u.Avatar,
      })
      .Where(u => u.Email.Contains(searchKeyword) || u.FullName.Contains(searchKeyword) || u.Phone != null && u.Phone.Contains(searchKeyword));

    int totalCount = query.Count();

    var users = query
      .OrderByDescending(user => user.FullName)
      .Skip(req.Page * req.PageSize)
      .Take(req.PageSize)
      .ToList();

    return new UserList
    {
      Users = users,
      TotalCount = totalCount
    };
  }

  public int UpdateUser(User user, IEnumerable<Media> mediaList)
  {
    // clear old media
    var removeMedia = user.ListMedia.Where(m => !mediaList.Select(m => m.Id).Contains(m.Id)).ToList();
    var newMedia = mediaList.Where(m => !user.ListMedia.Select(m => m.Id).Contains(m.Id)).ToList();

    if (removeMedia.Count > 0)
    {
      _dbContext.Medias.RemoveRange(removeMedia);
      user.ListMedia.RemoveAll(m => removeMedia.Select(m => m.Id).Contains(m.Id));
    }

    _dbContext.Medias.AddRange(newMedia);
    user.ListMedia.AddRange(newMedia);

    _dbContext.Users.Update(user);
    return _dbContext.SaveChanges();
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
