using art_tattoo_be.Application.DTOs.Shift;
using art_tattoo_be.Domain.Booking;
using art_tattoo_be.Infrastructure.Database;
using art_tattoo_be.Infrastructure.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace art_tattoo_be.Application.Controllers;

[ApiController]
[Route("api/shift")]
public class ShiftController : ControllerBase
{
  private readonly ILogger<ShiftController> _logger;
  private readonly IShiftRepository _shiftRepo;

  private readonly IMapper _mapper;

  public ShiftController(ILogger<ShiftController> logger, ArtTattooDbContext dbContext, IMapper mapper)
  {
    _shiftRepo = new ShiftRepository(dbContext);
    _logger = logger;
    _mapper = mapper;
  }

  [HttpGet]
  public async Task<IActionResult> GetShifts([FromQuery] ShiftQuery query)
  {
    var result = _shiftRepo.GetAllAsync(query);
    return Ok(result);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetShiftById(Guid id)
  {
    var result = await _shiftRepo.GetByIdAsync(id);
    return Ok(result);
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(Guid id)
  {
    var result = await _shiftRepo.DeleteAsync(id);
    return Ok(result);
  }
}
