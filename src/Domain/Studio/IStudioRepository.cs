namespace art_tattoo_be.Domain.Studio;
using art_tattoo_be.Application.DTOs.Studio;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Media;


public interface IStudioRepository
{
  int Count();
  bool IsExist(Guid id);
  bool IsStudioUserExist(Guid userId);
  bool IsStudioUserExist(Guid userId, Guid studioId);
  Task<Studio?> GetAsync(Guid id);
  StudioUser? GetStudioUser(Guid id);
  Guid GetStudioIdByUserId(Guid userId);
  IEnumerable<StudioWorkingTime> GetStudioWorkingTime(Guid studioId);
  IEnumerable<Studio> GetStudios();
  IEnumerable<StudioUser> GetStudioArtist(Guid studioId);
  StudioUserList GetStudioUsers(GetStudioUserQuery req);
  StudioList GetStudioPages(GetStudioQuery req);
  Task<int> CreateAsync(Studio studio);
  Task<int> CreateStudioUserAsync(StudioUser studioUser, int roleId);
  int Update(Studio studio, IEnumerable<Media> mediaList);
  int UpdateStudioStatus(Guid id, StudioStatusEnum status);
  int UpdateStudioUser(Guid id, UpdateStudioUserReq req);
  int DeleteStudio(Guid id);
  int DeleteStudioUser(Guid id);
}
