using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using art_tattoo_be.Domain.Testimonial;
using art_tattoo_be.Infrastructure.Database;
using art_tattoo_be.src.Application.DTOs.Testimonial;
using AutoMapper;

namespace art_tattoo_be.src.Infrastructure.Repository
{
  public class TestomonialRepository : ITestimonialRepository
  {
    private readonly ArtTattooDbContext _dbContext;
    private readonly IMapper _mapper;
    public TestimonialRepository(IMapper mapper, ArtTattooDbContext dbContext)
    {
      _mapper = mapper;
      _dbContext = dbContext;
    }
    public int CreateTestimonial(Testimonial testimonial)
    {
      throw new NotImplementedException();
    }

    public int DeleteTestimonial(Guid id)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Testimonial> GetAll()
    {
      return _dbContext.Testimonials;
    }

    public Testimonial GetById(Guid id)
    {
      throw new NotImplementedException();
    }

    public TestimonialList GetTestimonialPage(GetTestimonialQuery req)
    {
      throw new NotImplementedException();
    }

    public int UpdateTestimonial(Testimonial testimonial)
    {
      throw new NotImplementedException();
    }
  }
}