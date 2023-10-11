namespace art_tattoo_be.Application.DTOs.Studio;

using art_tattoo_be.Domain.Studio;
using AutoMapper;

public class StudioWorkingTimeDto
{
  public Guid Id { get; set; }
  public Guid StudioId { get; set; }
  public int DayOfWeek { get; set; }
  public TimeSpan OpenAt { get; set; }
  public TimeSpan CloseAt { get; set; }
}

public class StudioWorkingTimeCreate
{
  public int DayOfWeek { get; set; }
  public TimeSpan OpenAt { get; set; }
  public TimeSpan CloseAt { get; set; }
}

public class StudioWorkingTimeProfile : Profile
{
  public StudioWorkingTimeProfile()
  {
    CreateMap<StudioWorkingTime, StudioWorkingTimeDto>();
  }
}
