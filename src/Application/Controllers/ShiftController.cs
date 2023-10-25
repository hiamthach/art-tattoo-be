using art_tattoo_be.Domain.Booking;
using Microsoft.AspNetCore.Mvc;

namespace art_tattoo_be.Application.Controllers;

[ApiController]
[Route("api/shift")]
public class ShiftController : ControllerBase
{
  private readonly IShiftRepository _shiftRepo;

  public ShiftController(IShiftRepository shiftRepo)
  {
    _shiftRepo = shiftRepo;
  }

  [HttpGet]
  public async Task<IActionResult> Get()
  {
    var result = await _shiftRepo.GetAllAsync();
    return Ok(result);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> Get(int id)
  {
    var result = await _shiftRepo.GetByIdAsync(id);
    return Ok(result);
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    var result = await _shiftRepo.DeleteAsync(id);
    return Ok(result);
  }
}
