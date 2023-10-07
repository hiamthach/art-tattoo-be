using art_tattoo_be.Application.DTOs.Pagination;
using art_tattoo_be.Application.Shared.Enum;

namespace art_tattoo_be.Domain.Studio;

public interface IStudioRepository
{
  Task<Studio?> GetAsync(Guid id);
  Task<IEnumerable<Studio>> GetStudios();
  Task<IEnumerable<Studio>> GetStudioPages(PaginationReq req);
  Task<int> CreateAsync(Studio studio);
  Task<int> UpdateAsync(Studio studio);
  int UpdateStudioStatus(Guid id, StudioStatusEnum status);
  int DeleteStudio(Guid id);
}
