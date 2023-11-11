namespace art_tattoo_be.Application.DTOs.Invoice;

using art_tattoo_be.Application.DTOs.Appointment;
using art_tattoo_be.Application.DTOs.Pagination;
using art_tattoo_be.Application.DTOs.Studio;
using art_tattoo_be.Application.DTOs.User;
using art_tattoo_be.Application.Shared.Enum;
using art_tattoo_be.Domain.Invoice;
using art_tattoo_be.src.Application.DTOs.StudioService;
using AutoMapper;

public class InvoiceDto
{
  public Guid Id { get; set; }
  public Guid StudioId { get; set; }
  public Guid UserId { get; set; }
  public double Total { get; set; }
  public PayMethodEnum PayMethod { get; set; }
  public string? Notes { get; set; }
  public Guid? AppointmentId { get; set; }
  public DateTime CreatedAt { get; set; }
  public StudioDto Studio { get; set; } = null!;
  public UserDto User { get; set; } = null!;
  public AppointmentDto? Appointment { get; set; }
  public StudioServiceDto? Service { get; set; }
}

public class InvoiceResp : PaginationResp
{
  public List<InvoiceDto> Invoices { get; set; } = null!;
}

public class InvoiceProfile : Profile
{
  public InvoiceProfile()
  {
    CreateMap<Invoice, InvoiceDto>()
      .ForMember(dest => dest.Studio, opt => opt.MapFrom(src => src.Studio))
      .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
      .ForMember(dest => dest.Appointment, opt => opt.MapFrom(src => src.Appointment))
      .ForMember(dest => dest.Service, opt => opt.MapFrom(src => src.Service));
  }
}

