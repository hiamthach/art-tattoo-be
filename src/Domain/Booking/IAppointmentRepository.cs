namespace art_tattoo_be.Domain.Booking;

using art_tattoo_be.Application.DTOs.Analytics;
using art_tattoo_be.Application.DTOs.Appointment;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Media;

public interface IAppointmentRepository
{
  AdminMostPopularStudio? GetMostPopularStudio();
  BookingAdminDashboard GetBookingAdminDashboard();
  List<AdminBookingDaily> GetBookingDaily();
  AppointmentList GetAllAsync(AppointmentQuery query);
  Appointment? GetByIdAsync(Guid id);
  bool IsBooked(Guid shiftId, Guid userId);
  Task<int> CreateAsync(Appointment appointment);
  Task<int> UpdateAsync(Appointment appointment, IEnumerable<Media> mediaList);
  Task<int> UpdateAsync(Appointment appointment);
  Task<int> UpdateStatusAsync(Guid id, AppointmentStatusEnum status);
}
