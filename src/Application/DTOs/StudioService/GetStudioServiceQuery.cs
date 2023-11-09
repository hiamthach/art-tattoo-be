namespace art_tattoo_be.src.Application.DTOs.StudioService;

using art_tattoo_be.Application.DTOs.Pagination;

public class GetStudioServiceQuery : PaginationReq
{
  public Guid StudioId { get; set; }
  public string? SearchKeyword { get; set; } = null!;
}

public class GetStudioServiceReq : PaginationReq
{
  public Guid StudioId { get; set; }
  public string? SearchKeyword { get; set; } = null!;
  public bool IsStudio { get; set; }
}
