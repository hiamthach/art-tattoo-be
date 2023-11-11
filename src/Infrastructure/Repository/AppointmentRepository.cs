namespace art_tattoo_be.Infrastructure.Repository;

using art_tattoo_be.Application.DTOs.Appointment;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Booking;
using art_tattoo_be.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

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
    .Include(app => app.User)
    .Include(app => app.Artist).ThenInclude(a => a.User)
    .Include(app => app.Service)
    .Where(app =>
        (query.StudioId == null || app.Shift.StudioId == query.StudioId) &&
        (query.UserId == null || app.UserId == query.UserId) &&
        (query.StatusList == null || query.StatusList.Contains(app.Status)) &&
        (query.StartDate == null || app.Shift.Start >= query.StartDate) &&
        (query.EndDate == null || app.Shift.End <= query.EndDate) &&
        (query.SearchKeyword == null || app.User.FullName.Contains(query.SearchKeyword) || app.User.Email.Contains(query.SearchKeyword) || (app.User.Phone != null && app.User.Phone.Contains(query.SearchKeyword))) &&
        (query.ServiceList == null || (app.ServiceId != null && query.ServiceList.Contains(app.ServiceId.ToString())))
    );

    int totalCount = q.Count();

    var pagedResults = q
        .OrderBy(app => app.Shift.Start)
        .Skip(query.PageSize * query.Page)
        .Take(query.PageSize)
        .ToList();

    return new AppointmentList
    {
      TotalCount = totalCount,
      Appointments = pagedResults
    };
  }

  public Appointment? GetByIdAsync(Guid id)
  {
    return _dbContext.Appointments
      .Include(app => app.Shift).ThenInclude(s => s.ShiftUsers)
      .Include(app => app.Shift.Studio)
      .Include(app => app.User)
      .Include(app => app.Artist).ThenInclude(a => a.User)
      .Include(app => app.Service)
      .FirstOrDefault(a => a.Id == id);
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
