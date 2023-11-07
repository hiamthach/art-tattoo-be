using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using art_tattoo_be.Application.DTOs.Studio;
using art_tattoo_be.Application.Shared;
using art_tattoo_be.Application.Shared.Handler;
using art_tattoo_be.Domain.Category;
using art_tattoo_be.Domain.Media;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.Infrastructure.Cache;
using art_tattoo_be.Infrastructure.Database;
using art_tattoo_be.Infrastructure.Repository;
using art_tattoo_be.src.Application.DTOs.StudioService;
using art_tattoo_be.src.Domain.Studio;
using art_tattoo_be.src.Infrastructure.Repository;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace art_tattoo_be.src.Application.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/studioservice")]
    public class StudioServiceController : ControllerBase
    {
        private readonly ILogger<StudioServiceController> _logger;
        private readonly IStudioServiceRepository _stuserRepo;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _cateRepo;
        private readonly IStudioRepository _stuRepo;
        private readonly ICacheService _cacheService;

        public StudioServiceController(ILogger<StudioServiceController> logger, ArtTattooDbContext dbContext, IMapper mapper, ICacheService cacheService)
        {
            _logger = logger;
            _stuserRepo = new StudioServiceRepository(dbContext);
            _mapper = mapper;
            _cateRepo = new CategoryRepository(dbContext);
            _stuRepo = new StudioRepository(dbContext);
            _cacheService = cacheService;
        }
        [HttpPost()]
        public async Task<IActionResult> GetAll([FromBody] GetStudioServiceQuery req)
        {
            _logger.LogInformation("Get All Studio Service");
            try
            {
                var redisKey = $"studioServices{req.StudioId}:{req.Page}:{req.PageSize}";
                if (req.SearchKeyword != null)
                {
                    redisKey += $"?search={req.SearchKeyword}";
                }
                var studioServiceCache = await _cacheService.Get<StudioServiceResp>(redisKey);
                if (studioServiceCache != null)
                {
                    return Ok(studioServiceCache);
                }
                StudioServiceResp resp = new()
                {
                    Page = req.Page,
                    PageSize = req.PageSize
                };

                var studioServices = _stuserRepo.GetStudioServicePages(req);

                resp.Total = studioServices.TotalCount;

                resp.Data = _mapper.Map<List<StudioServiceDto>>(studioServices.StudioServices);

                await _cacheService.Set(redisKey, resp);

                return Ok(resp);
            }
            catch (Exception e)
            {
                return ErrorResp.SomethingWrong(e.Message);
            }


        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                var redisKey = $"studioService:{id}";
                var studioServiceCahe = await _cacheService.Get<StudioServiceDto>(redisKey);

                if (studioServiceCahe != null)
                {
                    return Ok(studioServiceCahe);
                }

                _logger.LogInformation("Get Studio Service by @id: ", id);
                var studioServiceDto = _mapper.Map<StudioServiceDto>(_stuserRepo.GetById(id));
                if (studioServiceDto == null)
                {
                    return ErrorResp.NotFound("Studio Service not found");
                }
                else
                {
                    await _cacheService.Set(redisKey, studioServiceDto);

                    return Ok(studioServiceDto);
                }
            }
            catch (Exception e)
            {
                return ErrorResp.SomethingWrong(e.Message);
            }
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateStudioService([FromBody] CreateStuioServiceReq body)
        {
            _logger.LogInformation("Create Studio Service: @body", body);
            try
            {
                var studio = await _stuRepo.GetAsync(body.StudioId);
                if (studio is not null && _cateRepo.GetById(body.CategoryId) is not null)
                {
                    var studioService = new StudioService
                    {
                        Id = Guid.NewGuid(),
                        StudioId = body.StudioId,
                        CategoryId = body.CategoryId,
                        Name = body.Name,
                        Description = body.Description,
                        MinPrice = body.MaxPrice,
                        MaxPrice = body.MaxPrice,
                        Discount = body.Discount,
                    };

                    studioService.ListMedia = body.ListMedia.Select(m =>
                    {
                        return new Media
                        {
                            Id = Guid.NewGuid(),
                            Url = m.Url,
                            Type = m.Type
                        };
                    }).ToList();

                    var result = _stuserRepo.CreateStudioService(studioService);

                    if (result > 0)
                    {
                        return Ok(new BaseResp { Message = "Create successfully", Success = true });
                    }
                    else
                    {
                        return ErrorResp.BadRequest("Create failed");
                    }
                }
                else
                {
                    return ErrorResp.NotFound("Could not find Studio or Category");
                }
            }
            catch (Exception e)
            {
                return ErrorResp.SomethingWrong(e.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudioService([FromRoute] Guid id)
        {
            _logger.LogInformation("Delete Studio Service Id: @id", id);
            try
            {
                var result = _stuserRepo.DeleteStudioService(id);
                if (result > 0)
                {
                    var redisKey = $"studioService:{id}";
                    await _cacheService.Remove(redisKey);
                    await _cacheService.ClearWithPattern("studioServices");

                    return Ok(new BaseResp { Message = "Delete Succesfully", Success = true });
                }
                else
                {
                    return ErrorResp.BadRequest("Delete failed");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error delete Studio Service");
                return ErrorResp.SomethingWrong(e.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudioService([FromBody] UpdateStudioServiceReq req, [FromRoute] Guid id)
        {
            _logger.LogInformation("Update Studio Service @id", id);
            try
            {
                var studioService = _stuserRepo.GetById(id);
                if (studioService == null)
                {
                    return ErrorResp.NotFound("Studio Service not found");
                }
                var studioServiceMapped = _mapper.Map(req, studioService);
                var mediaList = new List<Media>();
                mediaList.AddRange(studioServiceMapped.ListMedia);
                if (req.ListNewMedia != null)
                {
                    var newMedia = req.ListNewMedia.Select(m =>
                    {
                        return new Media
                        {
                            Id = Guid.NewGuid(),
                            Url = m.Url,
                            Type = m.Type
                        };
                    }).ToList();
                    mediaList.AddRange(newMedia);
                }
                if (req.ListRemoveMedia != null)
                {
                    var removeMedia = studioServiceMapped.ListMedia.Where(m => req.ListRemoveMedia.Contains(m.Id.ToString())).ToList();
                    mediaList.RemoveAll(m => removeMedia.Select(m => m.Id).Contains(m.Id));
                }

                var result = _stuserRepo.UpddateStudioService(studioServiceMapped, mediaList);

                if (result > 0)
                {
                    var redisKey = $"studioService:{id}";
                    await _cacheService.Remove(redisKey);
                    await _cacheService.ClearWithPattern("studioServices");

                    return Ok(new BaseResp
                    {
                        Message = "Update Studio Service successfully",
                        Success = true
                    });
                }
                else
                {
                    return ErrorResp.BadRequest("Update Studio Service failed");
                }
            }
            catch (Exception e)
            {
                return ErrorResp.SomethingWrong(e.Message);
            }
        }
    }
}