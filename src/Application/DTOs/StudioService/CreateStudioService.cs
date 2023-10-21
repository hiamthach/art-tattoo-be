using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.src.Application.DTOs.StudioService;
using AutoMapper;


namespace art_tattoo_be.src.Application.DTOs.StudioService
{
    public class CreateStuioServiceReq
    {
        public Guid StudioId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        [Required]
        [Range(1,double.MaxValue,ErrorMessage ="Min Price must larger than 0")]
        public double MinPrice { get; set; }
        [Required]
        [Range(1,double.MaxValue,ErrorMessage ="Max Price must larger than 0")]
        public double MaxPrice { get; set; }
        public double Discount { get; set; }
    }
}
