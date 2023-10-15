namespace art_tattoo_be.Domain.Studio;

using art_tattoo_be.Application.DTOs.Studio;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Media;


public interface IStudioRepository
{
  int Count();
  Task<Studio?> GetAsync(Guid id);
  IEnumerable<Studio> GetStudios();
  StudioList GetStudioPages(GetStudioQuery req);
  Task<int> CreateAsync(Studio studio);
  int Update(Studio studio, IEnumerable<Media> mediaList);
  int UpdateStudioStatus(Guid id, StudioStatusEnum status);
  int DeleteStudio(Guid id);
}
