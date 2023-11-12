using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using art_tattoo_be.Domain.Testimonial;
using art_tattoo_be.Infrastructure.Database;
using art_tattoo_be.src.Application.DTOs.Testimonial;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace art_tattoo_be.src.Infrastructure.Repository
{
  public class TestimonialRepository : ITestimonialRepository
  {
    private readonly ArtTattooDbContext _dbContext;
    public TestimonialRepository(ArtTattooDbContext dbContext)
    {
      _dbContext = dbContext;
    }
    public int CreateTestimonial(Testimonial testimonial)
    {
      _dbContext.Testimonials.Add(testimonial);
      return _dbContext.SaveChanges();
    }

    public int DeleteTestimonial(Guid id)
    {
      var testimonial = _dbContext.Testimonials.Find(id) ?? throw new Exception("Testimonial not found");
      _dbContext.Remove(testimonial);
      return _dbContext.SaveChanges();
    }

    public Testimonial GetById(Guid userId, Guid id)
    {
      return _dbContext.Testimonials
      .Where(tes => tes.CreatedBy == userId)
      .Where(tes => tes.Id == id)
      .Include(tes => tes.User)
      .FirstOrDefault() ?? throw new Exception("Testimonial not found");
    }

    public TestimonialList GetTestimonialPage(GetTestimonialQuery req)
    {
      var query = _dbContext.Testimonials
      .Where(tes => req.StudioId == null || tes.StudioId == req.StudioId);
      int totalCount = query.Count();
      var testimonial = query
      .Include(tes => tes.User)
      .Select(tes => new Testimonial
      {
        Id = tes.Id,
        StudioId = tes.StudioId,
        Title = tes.Title,
        Content = tes.Content,
        Rating = tes.Rating,
        CreatedBy = tes.CreatedBy,
        CreatedAt = tes.CreatedAt,
        UpdatedAt = tes.UpdatedAt,
        User = tes.User
      })
      .OrderByDescending(tes => tes.CreatedAt)
      .Skip(req.Page * req.PageSize)
      .Take(req.PageSize)
      .ToList();
      return new TestimonialList
      {
        Testimonials = testimonial,
        TotalCount = totalCount
      };
    }

    public TestimonialList GetTestimonialPageByUser(GetTestimonialQuery req, Guid userId)
    {
      var query = _dbContext.Testimonials
      .Where(tes => tes.StudioId == req.StudioId)
      .Where(tes => tes.CreatedBy == userId);
      int totalCount = query.Count();
      var testimonial = query
      .Include(tes => tes.User)
      .Select(tes => new Testimonial
      {
        Id = tes.Id,
        StudioId = tes.StudioId,
        Title = tes.Title,
        Content = tes.Content,
        Rating = tes.Rating,
        CreatedBy = tes.CreatedBy,
        CreatedAt = tes.CreatedAt,
        UpdatedAt = tes.UpdatedAt,
        User = tes.User
      })
      .OrderByDescending(tes => tes.CreatedAt)
      .Skip(req.Page * req.PageSize)
      .Take(req.PageSize)
      .ToList();
      return new TestimonialList
      {
        Testimonials = testimonial,
        TotalCount = totalCount
      };
    }

    public int UpdateTestimonial(Testimonial testimonial)
    {
      _dbContext.Testimonials.Update(testimonial);
      return _dbContext.SaveChanges();
    }

    public IEnumerable<Testimonial> GetAll()
    {
      return _dbContext.Testimonials.ToList();
    }
  }
}