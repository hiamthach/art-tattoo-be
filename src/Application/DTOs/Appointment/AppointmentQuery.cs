using art_tattoo_be.Application.DTOs.Pagination;
using art_tattoo_be.Application.Shared.Enum;

namespace art_tattoo_be.Application.DTOs.Appointment;

public class AppointmentQuery : PaginationReq
{
  public Guid? StudioId { get; set; }
  public Guid? UserId { get; set; }
  public List<AppointmentStatusEnum>? StatusList { get; set; }
  public DateTime? StartDate { get; set; }
  public DateTime? EndDate { get; set; }
  public Guid? ArtistId { get; set; }
  public string? SearchKeyword { get; set; }
  public List<string>? ServiceList { get; set; }
}

public class GetAppointmentsQuery : PaginationReq
{
  public Guid? StudioId { get; set; }
  public List<AppointmentStatusEnum>? StatusList { get; set; }
  public DateTime? StartDate { get; set; }
  public DateTime? EndDate { get; set; }
  public string? SearchKeyword { get; set; }
  public List<string>? ServiceList { get; set; }
}

public class GetStudioAppointmentsQuery : PaginationReq
{
  public List<AppointmentStatusEnum>? StatusList { get; set; }
  public DateTime? StartDate { get; set; }
  public DateTime? EndDate { get; set; }
  public Guid? UserId { get; set; }
  public Guid? ArtistId { get; set; }
  public string? SearchKeyword { get; set; }
  public List<string>? ServiceList { get; set; }
}
