using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace art_tattoo_be.src.Application.DTOs.Testimonial
{
    public class CreateTestimonialReq
    {
        Guid StudioId { get; set; }
        string Title { get; set; } = null!;
        string Content { get; set; } = null!;
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Rating must larger than 0")]
        double Rating { get; set; }
        Guid CreatedBy { get; set; }
    }
}