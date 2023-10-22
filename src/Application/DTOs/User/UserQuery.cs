using art_tattoo_be.Application.DTOs.Pagination;

namespace art_tattoo_be.Application.DTOs.User;

public class GetUserQuery : PaginationReq
{
  public string? SearchKeyword { get; set; } = null!;
}
