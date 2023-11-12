using art_tattoo_be.Application.DTOs.Pagination;
using art_tattoo_be.Application.Shared.Enum;

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
  public int? CategoryId { get; set; }
  // Rating must be 1, 2, 3, 4, 5
  public List<int>? RatingList { get; set; }
}

public class StudioQuery : PaginationReq
{
  public ViewPort? ViewPortNE { get; set; }
  public ViewPort? ViewPortSW { get; set; }
  public string? SearchKeyword { get; set; } = null!;
  public int? CategoryId { get; set; }
  public List<StudioStatusEnum>? StatusList { get; set; }
  public List<int>? RatingList { get; set; }
  public bool IsAdmin { get; set; }
}

public class GetStudioAdminQuery : PaginationReq
{
  public ViewPort? ViewPortNE { get; set; }
  public ViewPort? ViewPortSW { get; set; }
  public string? SearchKeyword { get; set; } = null!;
  public int? CategoryId { get; set; }
  public List<StudioStatusEnum>? StatusList { get; set; }
  public List<int>? RatingList { get; set; }
}

public class GetStudioUserQuery : PaginationReq
{
  public Guid StudioId { get; set; }
  public string? SearchKeyword { get; set; } = null!;
}
