namespace art_tattoo_be.Application.DTOs.Invoice;

using art_tattoo_be.Application.DTOs.Pagination;
using art_tattoo_be.Application.Shared.Enum;

public class InvoiceQuery : PaginationReq
{
  public string? SearchKeyword { get; set; }
  public Guid? StudioId { get; set; }
  public Guid? UserId { get; set; }
  public List<string>? ServiceList { get; set; }
}

public class GetUserInvoiceQuery : PaginationReq
{
  public string? SearchKeyword { get; set; }
  public List<string>? ServiceList { get; set; }
}
