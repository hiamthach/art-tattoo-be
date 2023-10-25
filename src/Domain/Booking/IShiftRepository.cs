namespace art_tattoo_be.Domain.Booking;

public interface IShiftRepository
{
  Task<IEnumerable<Shift>> GetAllAsync();
  Task<IEnumerable<Shift>> GetByDateAsync(DateTime date);
  Task<IEnumerable<Shift>> GetByDateRangeAsync(DateTime start, DateTime end);
  Task<Shift> GetByIdAsync(int id);
  Task<Shift> CreateAsync(Shift shift);
  Task<Shift> UpdateAsync(int id, Shift shift);
  Task<Shift> DeleteAsync(int id);
}
