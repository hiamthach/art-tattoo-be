namespace art_tattoo_be.Domain.Booking;

using art_tattoo_be.Application.DTOs.Analytics;
using art_tattoo_be.Application.DTOs.Appointment;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Media;

public interface IAppointmentRepository
{
  AdminMostPopularStudio? GetMostPopularStudio();
  BookingAdminDashboard GetBookingAdminDashboard();
  BookingAdminDashboard GetBookingStudioDashboard(Guid studioId);
  UserAdminDashboard GetUserBookingDashboard(Guid studioId);
  StudioMostPopularArtist? GetMostPopularArtist(Guid studioId);
  List<AdminBookingDaily> GetBookingDaily();
  List<AdminBookingDaily> GetBookingDaily(Guid studioId);
  AppointmentList GetAllAsync(AppointmentQuery query);
  Appointment? GetByIdAsync(Guid id);
  bool IsBooked(Guid shiftId, Guid userId);
  Task<int> CreateAsync(Appointment appointment);
  Task<int> UpdateAsync(Appointment appointment, IEnumerable<Media> mediaList);
  Task<int> UpdateAsync(Appointment appointment);
  Task<int> UpdateStatusAsync(Guid id, AppointmentStatusEnum status);
}
