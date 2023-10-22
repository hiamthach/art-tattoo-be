using art_tattoo_be.Application.DTOs.Pagination;

namespace art_tattoo_be.Application.DTOs.Studio;

public class ViewPort
{
  public double Lat { get; set; }
  public double Lng { get; set; }
}

public class GetStudioQuery : PaginationReq
{
  public ViewPort? ViewPortNE { get; set; }
  public ViewPort? ViewPortSW { get; set; }
  public string? SearchKeyword { get; set; } = null!;
}

public class GetStudioUserQuery : PaginationReq
{
  public Guid StudioId { get; set; }
  public string? SearchKeyword { get; set; } = null!;
}
