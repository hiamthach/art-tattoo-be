namespace art_tattoo_be.Domain.Testimonial;
using art_tattoo_be.src.Application.DTOs.Testimonial;

    public interface ITestimonialRepository
    {
        IEnumerable<Testimonial> GetAll();
        Testimonial GetById(Guid id);
        int CreateTestimonial(Testimonial testimonial);
        int DeleteTestimonial(Guid id);
        TestimonialList GetTestimonialPage(GetTestimonialQuery req);
        int UpdateTestimonial(Testimonial testimonial);
    }
