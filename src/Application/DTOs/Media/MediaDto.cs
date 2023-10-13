namespace art_tattoo_be.Application.DTOs.Media;

using art_tattoo_be.Domain.Media;
using art_tattoo_be.Application.Shared.Enum;
using AutoMapper;

public class MediaDto
{
  public Guid Id { get; set; }
  public string Url { get; set; } = null!;
  public MediaTypeEnum Type { get; set; }
}

public class MediaProfile : Profile
{
  public MediaProfile()
  {
    CreateMap<Media, MediaDto>();
  }
}

public class MediaCreate
{
  public string Url { get; set; } = null!;
  public MediaTypeEnum Type { get; set; } = MediaTypeEnum.Image;
}
