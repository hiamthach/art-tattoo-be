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

    return _dbContext.Shifts
      .Include(s => s.Artists)
      .Where(s => s.Start >= query.Start && s.End <= query.End)
      .Where(s => query.ArtistId == null || s.Artists.Any(a => a.Id == query.ArtistId))
      .Where(s => query.StudioId == null || s.StudioId == query.StudioId)
      .ToList();
  }

  public Task<Shift?> GetByIdAsync(Guid id)
  {
    return _dbContext.Shifts.Include(s => s.Artists).FirstOrDefaultAsync(s => s.Id == id);
  }


  public async Task<int> CreateAsync(Shift shift)
  {
    await _dbContext.Shifts.AddAsync(shift);

    return await _dbContext.SaveChangesAsync();
  }

  public async Task<int> UpdateAsync(Guid id, Shift shift)
  {
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
}
