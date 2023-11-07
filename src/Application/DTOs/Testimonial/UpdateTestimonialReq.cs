using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace art_tattoo_be.src.Application.DTOs.Testimonial
{
    public class UpdateTestimonialReq
    {
        public Guid StudioId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public double Rating { get; set; }
    }
}