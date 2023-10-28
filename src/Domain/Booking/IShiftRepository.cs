using art_tattoo_be.Application.DTOs.Shift;

namespace art_tattoo_be.Domain.Booking;

public interface IShiftRepository
{
  IEnumerable<Shift> GetAllAsync(ShiftQuery query);
  Task<Shift?> GetByIdAsync(Guid id);
  Task<int> CreateAsync(Shift shift);
  Task<int> CreateAsync(IEnumerable<Shift> shifts);
  Task<int> UpdateAsync(Guid id, Shift shift);
  Task<int> DeleteAsync(Guid id);
}
