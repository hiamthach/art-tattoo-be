using art_tattoo_be.Application.DTOs.Pagination;

namespace art_tattoo_be.Application.DTOs.User;

public class UserResp : PaginationResp
{
  public List<UserDto> Data { get; set; } = null!;
}
