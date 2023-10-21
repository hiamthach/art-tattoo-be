using art_tattoo_be.Application.DTOs.Studio;
using art_tattoo_be.Application.Shared.Enum;

namespace art_tattoo_be.Domain.Studio;

public interface IStudioRepository
{
  int Count();
  bool IsExist(Guid id);
  Task<Studio?> GetAsync(Guid id);
  IEnumerable<Studio> GetStudios();
  IEnumerable<StudioUser> GetStudioUsers(Guid studioId);
  StudioList GetStudioPages(GetStudioQuery req);
  Task<int> CreateAsync(Studio studio);
  Task<int> CreateStudioUserAsync(StudioUser studioUser);
  int Update(Studio studio);
  int UpdateStudioStatus(Guid id, StudioStatusEnum status);
  int UpdateStudioUserStatus(Guid id, bool status);
  int DeleteStudio(Guid id);
}
