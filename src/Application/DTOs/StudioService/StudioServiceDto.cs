namespace art_tattoo_be.src.Application.DTOs.StudioService;

using art_tattoo_be.Application.DTOs.Category;
using art_tattoo_be.Application.DTOs.Media;

using art_tattoo_be.Domain.Studio;
using AutoMapper;

public class StudioServiceDto
{
  public Guid Id { get; set; }
  public Guid StudioId { get; set; }
  public int CategoryId { get; set; }
  public string Name { get; set; } = null!;
  public string Description { get; set; } = null!;
  public double MinPrice { get; set; }
  public double MaxPrice { get; set; }
  public double Discount { get; set; }
  public bool IsDisabled { get; set; }
  public string Thumbnail { get; set; } = null!;
  public TimeSpan? ExpectDuration { get; set; }
  public CategoryDto Category { get; set; } = new();
  public List<MediaDto> ListMedia { get; set; } = new();
}

public class StudioServiceProfile : Profile
{
  public StudioServiceProfile()
  {
    CreateMap<StudioService, StudioServiceDto>()
        .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
        .ForMember(dest => dest.ListMedia, opt => opt.MapFrom(src => src.ListMedia));

    CreateMap<UpdateStudioServiceReq, StudioService>()
      .ForAllMembers(opts =>
      {
        opts.Condition((src, dest, srcMember) => srcMember != null);
      });
  }
}
