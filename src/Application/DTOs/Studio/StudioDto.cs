namespace art_tattoo_be.Application.DTOs.Studio;

using art_tattoo_be.Application.DTOs.Media;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.src.Application.DTOs.StudioService;
using AutoMapper;

public class StudioDto
{
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;
  public string? Slogan { get; set; }
  public string? Introduction { get; set; }
  public string? Detail { get; set; }
  public string? Logo { get; set; }
  public string Phone { get; set; } = null!;
  public string Email { get; set; } = null!;
  public string? Website { get; set; }
  public string? Facebook { get; set; }
  public string? Instagram { get; set; }
  public string Address { get; set; } = null!;
  public double Latitude { get; set; }
  public double Longitude { get; set; }
  public double Rating { get; set; }
  public StudioStatusEnum Status { get; set; }
  public List<StudioWorkingTimeDto> WorkingTimes { get; set; } = new();
  public List<StudioServiceDto> Services { get; set; } = new();
  public List<MediaDto> ListMedia { get; set; } = new();
}

public class StudioProfile : Profile
{
  public StudioProfile()
  {
    CreateMap<Studio, StudioDto>()
      .ForMember(dest => dest.Services, opt => opt.MapFrom(src => src.Services));
    CreateMap<Studio, Studio>() // Map from Studio to Studio
      .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore updating the Id
      .ForMember(dest => dest.WorkingTimes, opt => opt.MapFrom(src => src.WorkingTimes)); // Map WorkingTimes

    CreateMap<UpdateStudioReq, Studio>()
      .ForMember(dest => dest.WorkingTimes, opt => opt.Ignore())
      .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

    CreateMap<CreateStudioReq, Studio>()
      .ForMember(dest => dest.WorkingTimes, opt => opt.Ignore())
      .ForMember(dest => dest.Id, opt => opt.Ignore())
      .ForMember(dest => dest.ListMedia, opt => opt.Ignore())
      .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

    CreateMap<BecomeStudioReq, Studio>()
      .ForMember(dest => dest.Id, opt => opt.Ignore())
      .ForMember(dest => dest.ListMedia, opt => opt.Ignore())
      .ForMember(dest => dest.WorkingTimes, opt => opt.Ignore())
      .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
  }
}


