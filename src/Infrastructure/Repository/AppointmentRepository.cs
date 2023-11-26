namespace art_tattoo_be.Infrastructure.Repository;

using System.Collections.Generic;
using art_tattoo_be.Application.DTOs.Analytics;
using art_tattoo_be.Application.DTOs.Appointment;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Booking;
using art_tattoo_be.Domain.Media;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Domain.User;
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
        (query.ArtistId == null || app.DoneBy == query.ArtistId) &&
        (query.UserId == null || app.UserId == query.UserId) &&
        (query.StatusList == null || query.StatusList.Contains(app.Status)) &&
        (query.StartDate == null || app.Shift.Start >= query.StartDate) &&
        (query.EndDate == null || app.Shift.End <= query.EndDate) &&
        (query.SearchKeyword == null || app.User.FullName.Contains(query.SearchKeyword) || app.User.Email.Contains(query.SearchKeyword) || (app.User.Phone != null && app.User.Phone.Contains(query.SearchKeyword))) &&
        (query.ServiceList == null || (app.ServiceId != null && query.ServiceList.Any(s => s == app.ServiceId.ToString())))
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
      .Include(app => app.ListMedia)
      .FirstOrDefault(a => a.Id == id);
  }

  public async Task<int> CreateAsync(Appointment appointment)
  {
    await _dbContext.Appointments.AddAsync(appointment);
    return await _dbContext.SaveChangesAsync();
  }

  public Task<int> UpdateAsync(Appointment appointment, IEnumerable<Media> mediaList)
  {
    // clear old media
    var removeMedia = appointment.ListMedia.Where(m => !mediaList.Select(m => m.Id).Contains(m.Id)).ToList();
    var newMedia = mediaList.Where(m => !appointment.ListMedia.Select(m => m.Id).Contains(m.Id)).ToList();

    if (removeMedia.Count > 0)
    {
      _dbContext.Medias.RemoveRange(removeMedia);
      appointment.ListMedia.RemoveAll(m => removeMedia.Select(m => m.Id).Contains(m.Id));
    }

    _dbContext.Medias.AddRange(newMedia);
    appointment.ListMedia.AddRange(newMedia);

    _dbContext.Update(appointment);
    return _dbContext.SaveChangesAsync();
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

  public BookingAdminDashboard GetBookingAdminDashboard()
  {
    var totalAppointment = _dbContext.Appointments.Count();
    var totalAppointmentThisMonth = _dbContext.Appointments.Count(app => app.Shift.Start.Month == DateTime.Now.Month);
    var totalAppointmentLastMonth = _dbContext.Appointments.Count(app => app.Shift.Start.Month == DateTime.Now.AddMonths(-1).Month);

    return new BookingAdminDashboard
    {
      TotalBooking = totalAppointment,
      TotalBookingThisMonth = totalAppointmentThisMonth,
      TotalBookingLastMonth = totalAppointmentLastMonth,
    };
  }

  public List<AdminBookingDaily> GetBookingDaily()
  {
    // get data of last 7 days not include today
    var last7Days = Enumerable.Range(1, 7).Select(i => DateTime.Now.AddDays(-i)).ToList();

    // if not return 0 for that day

    var bookingDaily = _dbContext.Appointments
      .Where(app => last7Days.Contains(app.Shift.Start.Date))
      .GroupBy(app => app.Shift.Start.Date)
      .Select(g => new AdminBookingDaily
      {
        Times = g.Count(),
        Date = g.Key.ToString("yyyy-MM-dd")
      })
      .ToList();

    // fill missing date
    foreach (var day in last7Days)
    {
      if (!bookingDaily.Any(b => b.Date == day.ToString("yyyy-MM-dd")))
      {
        bookingDaily.Add(new AdminBookingDaily
        {
          Times = 0,
          Date = day.ToString("yyyy-MM-dd")
        });
      }
    }

    // sort by date
    bookingDaily = bookingDaily.OrderBy(b => b.Date).ToList();

    return bookingDaily;
  }

  public AdminMostPopularStudio? GetMostPopularStudio()
  {
    var mostPopularStudio = _dbContext.Appointments
      .Include(app => app.Shift)
      .Where(app => app.Shift.Start.Month == DateTime.Now.Month)
      .GroupBy(app => app.Shift.StudioId)
      .Select(g => new AdminMostPopularStudio
      {
        StudioId = g.Key,
        TotalBooking = g.Count(),
      })
      .OrderByDescending(g => g.TotalBooking)
      .FirstOrDefault();

    if (mostPopularStudio != null)
    {
      // get total revenue in this month
      mostPopularStudio.TotalRevenue = _dbContext.Invoices
        .Where(i => i.StudioId == mostPopularStudio.StudioId && i.CreatedAt.Month == DateTime.Now.Month)
        .Sum(i => i.Total);

      // get studio info
      mostPopularStudio.Studio = _dbContext.Studios
        .Where(stu => stu.Id == mostPopularStudio.StudioId)
        .Select(stu => new Studio
        {
          Id = stu.Id,
          Name = stu.Name,
          Logo = stu.Logo,
          Slogan = stu.Slogan,
          Rating = stu.Rating,
        })
        .FirstOrDefault();
    }

    return mostPopularStudio;
  }

  public BookingAdminDashboard GetBookingStudioDashboard(Guid studioId)
  {
    var totalAppointment = _dbContext.Appointments.Count();
    var totalAppointmentThisMonth = _dbContext.Appointments.Count(app => app.Shift.Start.Month == DateTime.Now.Month && app.Shift.StudioId == studioId);
    var totalAppointmentLastMonth = _dbContext.Appointments.Count(app => app.Shift.Start.Month == DateTime.Now.AddMonths(-1).Month && app.Shift.StudioId == studioId);

    return new BookingAdminDashboard
    {
      TotalBooking = totalAppointment,
      TotalBookingThisMonth = totalAppointmentThisMonth,
      TotalBookingLastMonth = totalAppointmentLastMonth,
    };
  }

  public UserAdminDashboard GetUserBookingDashboard(Guid studioId)
  {
    var totalUser = _dbContext.Appointments
      .Where(app => app.Shift.StudioId == studioId)
      .Select(app => app.UserId)
      .Distinct()
      .Count();

    var totalUserThisMonth = _dbContext.Appointments
      .Where(app => app.Shift.StudioId == studioId && app.Shift.Start.Month == DateTime.Now.Month)
      .Select(app => app.UserId)
      .Distinct()
      .Count();

    var totalUserLastMonth = _dbContext.Appointments
      .Where(app => app.Shift.StudioId == studioId && app.Shift.Start.Month == DateTime.Now.AddMonths(-1).Month)
      .Select(app => app.UserId)
      .Distinct()
      .Count();

    return new UserAdminDashboard
    {
      TotalUser = totalUser,
      TotalUserThisMonth = totalUserThisMonth,
      TotalUserLastMonth = totalUserLastMonth,
    };
  }

  public List<AdminBookingDaily> GetBookingDaily(Guid studioId)
  {
    // get data of last 5 days not include today
    var last7Days = Enumerable.Range(1, 7).Select(i => DateTime.Now.AddDays(-i)).ToList();

    // if not return 0 for that day

    var bookingDaily = _dbContext.Appointments
      .Where(app => last7Days.Contains(app.Shift.Start.Date) && app.Shift.StudioId == studioId)
      .GroupBy(app => app.Shift.Start.Date)
      .Select(g => new AdminBookingDaily
      {
        Times = g.Count(),
        Date = g.Key.ToString("yyyy-MM-dd")
      })
      .ToList();

    // fill missing date
    foreach (var day in last7Days)
    {
      if (!bookingDaily.Any(b => b.Date == day.ToString("yyyy-MM-dd")))
      {
        bookingDaily.Add(new AdminBookingDaily
        {
          Times = 0,
          Date = day.ToString("yyyy-MM-dd")
        });
      }
    }

    // sort by date
    bookingDaily = bookingDaily.OrderBy(b => b.Date).ToList();

    return bookingDaily;
  }

  public StudioMostPopularArtist? GetMostPopularArtist(Guid studioId)
  {
    var mostPopularArtist = _dbContext.Appointments
      .Include(app => app.Artist)
      .Include(app => app.Shift)
      .Where(app => app.Shift.Start.Month == DateTime.Now.Month && app.Shift.StudioId == studioId && app.DoneBy != null && app.DoneBy != Guid.Empty)
      .GroupBy(app => app.DoneBy)
      .Select(g => new StudioMostPopularArtist
      {
        ArtistId = g.Key ?? Guid.Empty,
        TotalBooking = g.Count(),
      })
      .OrderByDescending(g => g.TotalBooking)
      .FirstOrDefault();

    if (mostPopularArtist != null)
    {
      // get total revenue in this month
      mostPopularArtist.TotalRevenue = _dbContext.Invoices
        .Where(i => i.StudioId == studioId && i.CreatedAt.Month == DateTime.Now.Month && i.Appointment.DoneBy == mostPopularArtist.ArtistId)
        .Sum(i => i.Total);

      // get artist info
      mostPopularArtist.Artist = _dbContext.StudioUsers
        .Where(a => a.Id == mostPopularArtist.ArtistId)
        .Select(a => new StudioUser
        {
          Id = a.Id,
          UserId = a.UserId,
          StudioId = a.StudioId,
          User = new User
          {
            Id = a.User.Id,
            FullName = a.User.FullName,
            Email = a.User.Email,
            Phone = a.User.Phone,
            Avatar = a.User.Avatar,
          }
        })
        .FirstOrDefault();
    }

    return mostPopularArtist;
  }
}
