using art_tattoo_be.Application.DTOs.Appointment;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Booking;
using art_tattoo_be.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace art_tattoo_be.Infrastructure.Repository;

public class AppointmentRepository : IAppointmentRepository
{
  private readonly ArtTattooDbContext _dbContext;

  public AppointmentRepository(ArtTattooDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public AppointmentList GetAllAsync(AppointmentQuery query)
  {
    var q = _dbContext.Appointments
      .Include(app => app.Shift)
      .Where(app => query.StudioId == null || app.Shift.StudioId == query.StudioId)
      .Where(app => query.UserId == null || app.UserId == query.UserId);

    int totalCount = q.Count();

    var appointments = q
      .Skip(query.PageSize * query.Page)
      .Take(query.PageSize)
      .OrderByDescending(app => app.CreatedAt)
      .ToList();

    return new AppointmentList
    {
      TotalCount = totalCount,
      Appointments = appointments
    };
  }

  public Appointment? GetByIdAsync(Guid id)
  {
    return _dbContext.Appointments.Include(app => app.Shift).Include(a => a.Artist.User).FirstOrDefault(a => a.Id == id);
  }

  public async Task<int> CreateAsync(Appointment appointment)
  {
    await _dbContext.AddAsync(appointment);
    return await _dbContext.SaveChangesAsync();
  }

  public Task<int> UpdateAsync(Appointment appointment)
  {

    _dbContext.Update(appointment);
    return _dbContext.SaveChangesAsync();
  }

  public Task<int> UpdateStatusAsync(Guid id, AppointmentStatusEnum status)
  {
    var appointment = _dbContext.Appointments.Find(id) ?? throw new Exception("Appointment not found");
    appointment.Status = status;
    _dbContext.Update(appointment);
    return _dbContext.SaveChangesAsync();
  }

  public bool IsBooked(Guid shiftId, Guid userId)
  {
    return _dbContext.Appointments.Any(app => app.ShiftId == shiftId && app.UserId == userId);
  }
}