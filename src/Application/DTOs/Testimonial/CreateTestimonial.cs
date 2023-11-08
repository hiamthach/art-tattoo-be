using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace art_tattoo_be.src.Application.DTOs.Testimonial
{
    public class CreateTestimonialReq
    {
        public Guid StudioId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Rating must larger than 0")]
        public double Rating { get; set; }
        public Guid CreatedBy { get; set; }
    }
}