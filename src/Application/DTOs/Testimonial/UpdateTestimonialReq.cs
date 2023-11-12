using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace art_tattoo_be.src.Application.DTOs.Testimonial
{
    public class UpdateTestimonialReq
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        [Required]
        [Range(0, 5, ErrorMessage = "Rating must between 0 and 5")]
        public double Rating { get; set; }
    }
}