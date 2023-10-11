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
    CreateMap<Studio, Studio>() // Map from Studio to Studio
      .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore updating the Id
      .ForMember(dest => dest.WorkingTimes, opt => opt.MapFrom(src => src.WorkingTimes)); // Map WorkingTimes

    CreateMap<UpdateStudioReq, Studio>()
      .ForMember(dest => dest.WorkingTimes, opt => opt.Ignore())
      .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
  }
}


