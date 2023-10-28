namespace art_tattoo_be.Application.DTOs.Shift;

using AutoMapper;
using art_tattoo_be.Domain.Booking;

public class ShiftDto
{
  public Guid Id { get; set; }
  public DateTime Start { get; set; }
  public DateTime End { get; set; }
  public Guid StudioId { get; set; }
}

public class ShiftProfile : Profile
{
  public ShiftProfile()
  {
    CreateMap<Shift, ShiftDto>();
  }
}
