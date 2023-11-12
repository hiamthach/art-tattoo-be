namespace art_tattoo_be.Domain.Testimonial;
using art_tattoo_be.src.Application.DTOs.Testimonial;

public interface ITestimonialRepository
{
  TestimonialList GetTestimonialPageByUser(GetTestimonialQuery req, Guid userId);
  Testimonial GetById(Guid userId, Guid id);
  Testimonial? GetById(Guid id);
  int CreateTestimonial(Testimonial testimonial);
  int DeleteTestimonial(Guid id);
  TestimonialList GetTestimonialPage(GetTestimonialQuery req);
  int UpdateTestimonial(Testimonial testimonial);
  IEnumerable<Testimonial> GetAll();
  double GetRating(Guid studioId);
}
