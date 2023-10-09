namespace art_tattoo_be.Application.DTOs.Studio;

using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Studio;
using AutoMapper;

public class StudioDto
{
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;

  public string? Detail { get; set; }
  public string? Logo { get; set; }
  public string? Phone { get; set; }
  public string? Email { get; set; }
  public string? Website { get; set; }
  public string? Facebook { get; set; }
  public string? Instagram { get; set; }
  public string? Address { get; set; }
  public double Latitude { get; set; }
  public double Longitude { get; set; }
  public StudioStatusEnum Status { get; set; }

  public List<StudioWorkingTimeDto> WorkingTimes { get; set; } = new();
}

public class StudioProfile : Profile
{
  public StudioProfile()
  {
    CreateMap<Studio, StudioDto>();
  }
}


