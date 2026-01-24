using System.Security.Claims;
using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitCal.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public sealed class CalorieCalculatorController : ControllerBase
{
    private readonly ICalorieCalculatorService _service;
    private readonly IProfileService _profileService;

    public CalorieCalculatorController(
        ICalorieCalculatorService service,
        IProfileService profileService)
    {
        _service = service;
        _profileService = profileService;
    }

    private Guid GetAuthUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(idStr, out var userId))
            throw new UnauthorizedAccessException("Invalid user id");
        return userId;
    }

    [HttpPost("calculate")]
    public async Task<ActionResult<DailyCaloriesResultDTO>> Calculate([FromBody] DailyCaloriesRequestDTO request)
    {
        var result = await _service.CalculateDailyCaloriesAsync(request);

        var userId = GetAuthUserId();
        await _profileService.UpdateCaloriesAsync(userId, result.DailyCalories);

        return Ok(result);
    }
}