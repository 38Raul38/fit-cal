using System.Security.Claims;
using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Data.DTO.Response;
using FitCal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitCal.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FoodController : ControllerBase
{
    private readonly IFoodService _foodService;

    public FoodController(IFoodService foodService)
    {
        _foodService = foodService;
    }

    private Guid GetAuthUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(idStr, out var authUserId))
            throw new UnauthorizedAccessException("Invalid user id");
        return authUserId;
    }

    [HttpPost]
    [ProducesResponseType(typeof(FoodResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FoodResponseDTO>> CreateFood([FromBody] FoodRequestDTO request)
    {
        var authUserId = GetAuthUserId();
        var result = await _foodService.AddFoodAsync(authUserId, request);
        return CreatedAtAction(nameof(GetFoodById), new { id = result.FoodId }, result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(FoodResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FoodResponseDTO>> GetFoodById(int id)
    {
        var authUserId = GetAuthUserId();
        var result = await _foodService.GetFoodByIdAsync(authUserId, id);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<FoodResponseDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<FoodResponseDTO>>> GetAllFoods()
    {
        var authUserId = GetAuthUserId();
        var result = await _foodService.GetAllFoodsAsync(authUserId);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(FoodResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FoodResponseDTO>> UpdateFood(int id, [FromBody] FoodRequestDTO request)
    {
        var authUserId = GetAuthUserId();
        var result = await _foodService.UpdateFoodAsync(authUserId, id, request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteFood(int id)
    {
        var authUserId = GetAuthUserId();
        await _foodService.RemoveFoodAsync(authUserId, id);
        return NoContent();
    }
}
