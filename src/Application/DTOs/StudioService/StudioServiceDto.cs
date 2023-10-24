using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using art_tattoo_be.Domain.Category;
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
        public Category Catogory { get; set; } = new();
    }
}
public class StudioServiceProfile : Profile
{
    public StudioServiceProfile()
    {
        CreateMap<StudioService, StudioServiceDto>()
    .ForMember(dest => dest.Catogory, opt => opt.MapFrom(src => src.Category));
    }
}