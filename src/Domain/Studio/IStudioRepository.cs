using art_tattoo_be.Application.DTOs.Studio;
using art_tattoo_be.Application.Shared.Enum;

namespace art_tattoo_be.Domain.Studio;

public interface IStudioRepository
{
  int Count();
  bool IsExist(Guid id);
  bool IsStudioUserExist(Guid userId);
  Task<Studio?> GetAsync(Guid id);
  StudioUser? GetStudioUser(Guid id);
  IEnumerable<Studio> GetStudios();
  IEnumerable<StudioUser> GetStudioArtist(Guid studioId);
  StudioUserList GetStudioUsers(GetStudioUserQuery req);
  StudioList GetStudioPages(GetStudioQuery req);
  Task<int> CreateAsync(Studio studio);
  Task<int> CreateStudioUserAsync(StudioUser studioUser, int roleId);
  int Update(Studio studio);
  int UpdateStudioStatus(Guid id, StudioStatusEnum status);
  int UpdateStudioUser(Guid id, UpdateStudioUserReq req);
  int DeleteStudio(Guid id);
  int DeleteStudioUser(Guid id);
}
