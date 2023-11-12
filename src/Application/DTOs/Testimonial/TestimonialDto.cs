using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using art_tattoo_be.Application.DTOs.User;
using art_tattoo_be.Domain.Testimonial;
using art_tattoo_be.Domain.User;
using art_tattoo_be.src.Application.DTOs.Testimonial;
using AutoMapper;

namespace art_tattoo_be.src.Application.DTOs.Testimonial
{
    public class TestimonialDto
    {
        public Guid Id { get; set; }
        public Guid StudioId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public double Rating { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public UserDto UserDto {get; set;} = new();
    }
}
public class TestimonialProfile : Profile
{
    public TestimonialProfile()
    {
        CreateMap<Testimonial, TestimonialDto>()
        .ForMember(dest => dest.UserDto, opt => opt.MapFrom(src => src.User));
        CreateMap<UpdateTestimonialReq, Testimonial>()
        .ForMember(dest => dest.User, opt => opt.Ignore())
        .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}