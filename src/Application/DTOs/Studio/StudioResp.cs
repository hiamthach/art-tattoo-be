namespace art_tattoo_be.Application.DTOs.Studio;
using art_tattoo_be.Application.DTOs.Pagination;

public class StudioResp : PaginationResp
{
  public List<StudioDto> Data { get; set; } = new();
}

public class StudioUserResp : PaginationResp
{
  public List<StudioUserDto> Data { get; set; } = new();
}
