using art_tattoo_be.Application.DTOs.Pagination;
using art_tattoo_be.Application.Shared.Enum;

namespace art_tattoo_be.Domain.Studio;

public interface IStudioRepository
{
  int Count();
  Task<Studio?> GetAsync(Guid id);
  IEnumerable<Studio> GetStudios();
  IEnumerable<Studio> GetStudioPages(PaginationReq req);
  Task<int> CreateAsync(Studio studio);
  int Update(Studio studio);
  int UpdateStudioStatus(Guid id, StudioStatusEnum status);
  int DeleteStudio(Guid id);
}
