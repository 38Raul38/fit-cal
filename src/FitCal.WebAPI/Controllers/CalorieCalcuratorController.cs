using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace Nutrition.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CalorieCalculatorController : ControllerBase
{
    private readonly ICalorieCalculatorService _service;

    public CalorieCalculatorController(ICalorieCalculatorService service)
    {
        _service = service;
    }

    [HttpPost("calculate")]
    public async Task<ActionResult<DailyCaloriesResultDTO>> Calculate([FromBody] DailyCaloriesRequestDTO request)
    {
        var result = await _service.CalculateDailyCaloriesAsync(request);
        return Ok(result);
    }
}