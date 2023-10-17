using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.src.Application.DTOs.StudioService;
using AutoMapper;

namespace art_tattoo_be.src.Application.DTOs.StudioService
{
    public class StudioServiceDto
    {
        public Guid Id { get; set; }
        public Guid StudioId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public double Discount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
public class StudioServiceProfile : Profile
{
    public StudioServiceProfile()
    {
        CreateMap<StudioService, StudioServiceDto>();
    }
}