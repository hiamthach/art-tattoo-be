using art_tattoo_be.Application.DTOs.Pagination;

namespace art_tattoo_be.Application.DTOs.Appointment;

public class AppointmentQuery : PaginationReq
{
  public Guid? StudioId { get; set; }
  public Guid UserId { get; set; }
}

public class GetAppointmentsQuery : PaginationReq
{
  public Guid? StudioId { get; set; }
}
