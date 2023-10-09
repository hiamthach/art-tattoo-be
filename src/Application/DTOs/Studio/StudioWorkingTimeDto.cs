namespace art_tattoo_be.Application.DTOs.Studio;

using art_tattoo_be.Domain.Studio;
using AutoMapper;

public class StudioWorkingTimeDto
{
  public Guid Id { get; set; }
  public Guid StudioId { get; set; }
  public int DayOfWeek { get; set; }
  public DateTime OpenAt { get; set; }
  public DateTime CloseAt { get; set; }
}

public class StudioWorkingTimeProfile : Profile
{
  public StudioWorkingTimeProfile()
  {
    CreateMap<StudioWorkingTime, StudioWorkingTimeDto>();
  }
}
