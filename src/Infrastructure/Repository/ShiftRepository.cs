namespace art_tattoo_be.Infrastructure.Repository;

using art_tattoo_be.Application.DTOs.Shift;
using art_tattoo_be.Domain.Booking;
using art_tattoo_be.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

public class ShiftRepository : IShiftRepository
{
  private readonly ArtTattooDbContext _dbContext;

  public ShiftRepository(ArtTattooDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public IEnumerable<Shift> GetAllAsync(ShiftQuery query)
  {
    var q = _dbContext.Shifts
      .Include(s => s.ShiftUsers).ThenInclude(su => su.StudioUser).ThenInclude(su => su.User)
      .Where(s => s.Start >= query.Start && s.End <= query.End)
      .Where(s => s.StudioId == query.StudioId);

    if (query.ArtistId != null)
    {
      q = q.Where(s => s.ShiftUsers.Any(su => su.StuUserId == query.ArtistId && su.IsBooked == false));
    }

    return q
      .OrderBy(s => s.Start)
      .ToList();
  }

  public Task<Shift?> GetByIdAsync(Guid id)
  {
    return _dbContext.Shifts
      .Include(s => s.ShiftUsers).ThenInclude(su => su.StudioUser).ThenInclude(su => su.User)
      .FirstOrDefaultAsync(s => s.Id == id);
  }


  public async Task<int> CreateAsync(Shift shift)
  {
    await _dbContext.Shifts.AddAsync(shift);

    return await _dbContext.SaveChangesAsync();
  }

  public async Task<int> UpdateAsync(Guid id, Shift shift, UpdateShift req)
  {
    req.AssignArtists?.ForEach(a =>
      {
        var shiftUser = shift.ShiftUsers.FirstOrDefault(su => su.StuUserId == a);

        if (shiftUser == null)
        {
          shift.ShiftUsers.Add(new ShiftUser
          {
            ShiftId = id,
            StuUserId = a,
            IsBooked = false,
          });
        }
        else
        {
          shiftUser.IsBooked = false;
        }
      });

    req.UnassignArtists?.ForEach(a =>
    {
      var shiftUser = shift.ShiftUsers.FirstOrDefault(su => su.StuUserId == a);

      if (shiftUser != null)
      {
        shift.ShiftUsers.Remove(shiftUser);
      }
    });

    _dbContext.Update(shift);

    return await _dbContext.SaveChangesAsync();
  }

  public Task<int> DeleteAsync(Guid id)
  {
    var shift = _dbContext.Shifts.Find(id) ?? throw new Exception("Shift not found");

    _dbContext.Remove(shift);

    return _dbContext.SaveChangesAsync();
  }

  public Task<int> CreateAsync(IEnumerable<Shift> shifts)
  {
    _dbContext.Shifts.AddRange(shifts);

    return _dbContext.SaveChangesAsync();
  }

  public Task<int> RegisterUserAsync(Guid shiftId, Guid stuUserId)
  {
    var shift = _dbContext.Shifts.Find(shiftId) ?? throw new Exception("Shift not found");
    var stuUser = _dbContext.StudioUsers.Find(stuUserId) ?? throw new Exception("Studio user not found");

    shift.ShiftUsers.Add(new ShiftUser
    {
      ShiftId = shiftId,
      StuUserId = stuUserId,
      IsBooked = false,
    });

    return _dbContext.SaveChangesAsync();
  }

  public IEnumerable<Shift> GetShiftsByArtistId(Guid artistId)
  {
    return _dbContext.Shifts
      .Include(s => s.ShiftUsers)
      .Where(s => s.ShiftUsers.Any(su => su.StuUserId == artistId))
      .ToList();
  }

  public Task<ShiftUser?> GetShiftUserAsync(Guid shiftId, Guid artistId)
  {
    return _dbContext.ShiftUsers.FirstOrDefaultAsync(su => su.ShiftId == shiftId && su.StuUserId == artistId);
  }

  public Task<int> UpdateShiftUserAsync(ShiftUser shiftUser)
  {
    _dbContext.Update(shiftUser);

    return _dbContext.SaveChangesAsync();
  }
}
