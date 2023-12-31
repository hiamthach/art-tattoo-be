using art_tattoo_be.Application.Middlewares;
using art_tattoo_be.Application.Shared;
using art_tattoo_be.Application.Shared.Handler;
using art_tattoo_be.Core.Jwt;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Domain.Testimonial;
using art_tattoo_be.Domain.User;
using art_tattoo_be.Infrastructure.Cache;
using art_tattoo_be.Infrastructure.Database;
using art_tattoo_be.Infrastructure.Repository;
using art_tattoo_be.src.Application.DTOs.Testimonial;
using art_tattoo_be.src.Infrastructure.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace art_tattoo_be.src.Application.Controllers
{
  [Produces("application/json")]
  [ApiController]
  [Route("api/testimonial")]
  public class TestimonialController : ControllerBase
  {
    private readonly ILogger<TestimonialController> _logger;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;
    private readonly ITestimonialRepository _tesRepo;
    private readonly IStudioRepository _stuRepo;
    private readonly IUserRepository _userRepo;
    public TestimonialController(ILogger<TestimonialController> logger, ArtTattooDbContext dbContext, IMapper mapper, ICacheService cacheService)
    {
      _logger = logger;
      _tesRepo = new TestimonialRepository(dbContext);
      _stuRepo = new StudioRepository(dbContext);
      _userRepo = new UserRepository(dbContext);
      _mapper = mapper;
      _cacheService = cacheService;
    }
    [HttpPost()]
    public async Task<IActionResult> GetAll([FromBody] GetTestimonialQuery req)
    {
      _logger.LogInformation("Get All Testimonial");
      try
      {
        var redisKey = $"testimonials:{req.StudioId}:{req.Page}:{req.PageSize}";
        var testimonialCache = await _cacheService.Get<TestimonialResp>(redisKey);

        if (testimonialCache != null)
        {
          return Ok(testimonialCache);
        }
        TestimonialResp resp = new()
        {
          Page = req.Page,
          PageSize = req.PageSize
        };
        var testimonial = _tesRepo.GetTestimonialPage(req);

        resp.Total = testimonial.TotalCount;

        resp.Data = _mapper.Map<List<TestimonialDto>>(testimonial.Testimonials);

        await _cacheService.Set(redisKey, resp);

        return Ok(resp);
      }
      catch (Exception e)
      {
        return ErrorResp.SomethingWrong(e.Message);
      }
    }
    [Protected]
    [HttpPost("user")]
    public async Task<IActionResult> GetAllByUser([FromBody] GetTestimonialQuery req)
    {
      if (HttpContext.Items["payload"] is not Payload payload)
      {
        return ErrorResp.Unauthorized("Unauthorized");
      }

      _logger.LogInformation($"Get All Testimonial by User {payload.UserId}");
      try
      {
        var redisKey = $"testimonials:{payload.UserId}:{req.StudioId}:{req.Page}:{req.PageSize}";
        var testimonialCache = await _cacheService.Get<TestimonialResp>(redisKey);

        if (testimonialCache != null)
        {
          return Ok(testimonialCache);
        }
        TestimonialResp resp = new()
        {
          Page = req.Page,
          PageSize = req.PageSize
        };
        var testimonial = _tesRepo.GetTestimonialPageByUser(req, payload!.UserId);

        resp.Total = testimonial.TotalCount;

        resp.Data = _mapper.Map<List<TestimonialDto>>(testimonial.Testimonials);

        await _cacheService.Set(redisKey, resp);

        return Ok(resp);
      }
      catch (Exception e)
      {
        return ErrorResp.SomethingWrong(e.Message);
      }
    }
    [Protected]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
      if (HttpContext.Items["payload"] is not Payload payload)
      {
        return ErrorResp.Unauthorized("Unauthorized");
      }

      _logger.LogInformation($"Get Testimonial by User {payload.UserId}, Id: @id", id);
      try
      {
        var redisKey = $"testimonial:{payload.UserId}:{id}";
        var testimonialCache = await _cacheService.Get<TestimonialDto>(redisKey);
        if (testimonialCache != null)
        {
          return Ok(testimonialCache);
        }
        var testimonialDto = _mapper.Map<TestimonialDto>(_tesRepo.GetById(payload!.UserId, id));
        if (testimonialDto == null)
        {
          return ErrorResp.NotFound("Testimonial not found");
        }
        else
        {
          await _cacheService.Set(redisKey, testimonialDto);

          return Ok(testimonialDto);
        }
      }
      catch (Exception e)
      {
        return ErrorResp.SomethingWrong(e.Message);
      }
    }
    [Protected]
    [HttpPost("create")]
    public async Task<IActionResult> CreateTestimonial([FromBody] CreateTestimonialReq body)
    {
      if (HttpContext.Items["payload"] is not Payload payload)
      {
        return ErrorResp.Unauthorized("Unauthorized");
      }

      _logger.LogInformation($"Create Testimonial by User {payload.UserId}");
      try
      {
        var studio = await _stuRepo.GetAsync(body.StudioId);
        if (studio is null)
        {
          return ErrorResp.NotFound("Studio not found");
        }
        var testimonial = new Testimonial
        {
          Id = Guid.NewGuid(),
          StudioId = body.StudioId,
          Title = body.Title,
          Content = body.Content,
          Rating = body.Rating,
          CreatedBy = payload.UserId
        };

        var result = _tesRepo.CreateTestimonial(testimonial);
        if (result > 0)
        {
          var avgRating = _tesRepo.GetRating(body.StudioId);
          studio.Rating = avgRating;
          _stuRepo.Update(studio);

          await _cacheService.ClearWithPattern("testimonials");
          await _cacheService.ClearWithPattern($"studios");
          return Ok(new BaseResp
          {
            Message = "Created succesfully",
            Success = true
          });
        }
        else
        {
          return ErrorResp.BadRequest("Create failed");
        }
      }
      catch (Exception e)
      {
        return ErrorResp.SomethingWrong(e.Message);
      }
    }

    [Protected]
    [Permission(PermissionSlugConst.MANAGE_OWN_TESTIMONIAL, PermissionSlugConst.MANAGE_TESTIMONIAL)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTestimonial([FromRoute] Guid id)
    {
      if (HttpContext.Items["payload"] is not Payload payload)
      {
        return ErrorResp.Unauthorized("Unauthorized");
      }

      if (HttpContext.Items["permission"] is not string permission)
      {
        return ErrorResp.Forbidden($"Forbidden, you don't have permission");
      }

      _logger.LogInformation($"Delete Testimonial by User{payload.UserId}, Id: @id", id);
      try
      {
        var testimonial = _tesRepo.GetById(id);
        if (testimonial == null)
        {
          return ErrorResp.NotFound("Testimonial not found");
        }

        if (testimonial.CreatedBy != payload.UserId && permission != PermissionSlugConst.MANAGE_TESTIMONIAL)
        {
          return ErrorResp.Forbidden("Forbidden, you don't have permission");
        }

        var result = _tesRepo.DeleteTestimonial(id);

        if (result > 0)
        {
          var studio = await _stuRepo.GetAsync(testimonial.StudioId);
          var avgRating = _tesRepo.GetRating(testimonial.StudioId);
          if (studio != null)
          {
            studio.Rating = avgRating;
            _stuRepo.Update(studio);
          }
          var redisKey = $"testimonial:{payload.UserId}:{id}";
          await _cacheService.Remove(redisKey);
          await _cacheService.ClearWithPattern("testimonials");

          return Ok(new BaseResp
          {
            Message = "Deleted successfully",
            Success = true
          });
        }
        else
        {
          return ErrorResp.BadRequest("Delete failed");
        }
      }
      catch (Exception e)
      {
        return ErrorResp.SomethingWrong(e.Message);
      }
    }

    [Protected]
    [Permission(PermissionSlugConst.MANAGE_OWN_TESTIMONIAL, PermissionSlugConst.MANAGE_TESTIMONIAL)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTestimonial([FromBody] UpdateTestimonialReq req, [FromRoute] Guid id)
    {
      if (HttpContext.Items["payload"] is not Payload payload)
      {
        return ErrorResp.Unauthorized("Unauthorized");
      }

      if (HttpContext.Items["permission"] is not string permission)
      {
        return ErrorResp.Forbidden($"Forbidden, you don't have permission");
      }

      _logger.LogInformation($"Update Testimonial by User {payload.UserId}, Id: @id", id);
      try
      {
        var testimonial = _tesRepo.GetById(id);
        if (testimonial == null)
        {
          return ErrorResp.NotFound("Testimonial not found");
        }
        if (testimonial.CreatedBy != payload.UserId && permission != PermissionSlugConst.MANAGE_TESTIMONIAL)
        {
          return ErrorResp.Forbidden("Forbidden, you don't have permission");
        }

        var isRatingChanged = testimonial.Rating != req.Rating;
        var testimonialMapped = _mapper.Map(req, testimonial);
        var result = _tesRepo.UpdateTestimonial(testimonialMapped);
        if (result > 0)
        {
          if (isRatingChanged)
          {
            var studio = await _stuRepo.GetAsync(testimonialMapped.StudioId);
            var avgRating = _tesRepo.GetRating(testimonialMapped.StudioId);
            if (studio != null)
            {
              studio.Rating = avgRating;
              _stuRepo.Update(studio);
            }
          }
          var redisKey = $"testimonial:{payload.UserId}:{id}";
          await _cacheService.Remove(redisKey);
          await _cacheService.ClearWithPattern("testimonials");
          return Ok(new BaseResp
          {
            Message = "Updated successfully",
            Success = true
          });
        }
        else
        {
          return ErrorResp.BadRequest("Update failed");
        }
      }
      catch (Exception e)
      {
        return ErrorResp.SomethingWrong(e.Message);
      }
    }

    [HttpGet("rating/{studioId}")]
    public IActionResult GetAverageRatingStudio([FromRoute] Guid studioId)
    {
      _logger.LogInformation("Get Average Rating for Studio @id", studioId);
      try
      {
        double average = _tesRepo.GetAll()
        .Where(tes => tes.StudioId == studioId)
        .Average(tes => tes.Rating);
        return Ok(new { rating = average });
      }
      catch (Exception e)
      {
        return ErrorResp.SomethingWrong(e.Message);
      }
    }
    [HttpGet("rating")]
    public IActionResult GetAverageRating()
    {
      _logger.LogInformation("Get Average Rating for All Studio");
      try
      {
        double average = _tesRepo.GetAll().Average(tes => tes.Rating);
        return Ok(new { rating = average });
      }
      catch (Exception e)
      {
        return ErrorResp.SomethingWrong(e.Message);
      }
    }
  }
}
