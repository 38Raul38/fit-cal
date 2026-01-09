using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Data.DTO.Response;
using FitCal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FitCal.WebAPI.Middlewares;

[ApiController]
[Route("api/[controller]")]
public class FoodController : ControllerBase
{
    private readonly IFoodService _foodService;

    public FoodController(IFoodService foodService)
    {
        _foodService = foodService;
    }

    // POST: api/food
    [HttpPost]
    [ProducesResponseType(typeof(FoodResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FoodResponseDTO>> CreateFood([FromBody] FoodRequestDTO request)
    {
        var result = await _foodService.AddFoodAsync(request);
        return CreatedAtAction(nameof(GetFoodById), new { id = result.FoodId }, result);
    }

    // GET: api/food/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(FoodResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FoodResponseDTO>> GetFoodById(int id)
    {
        var result = await _foodService.GetFoodByIdAsync(id);
        return Ok(result);
    }

    // GET: api/food
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<FoodResponseDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<FoodResponseDTO>>> GetAllFoods()
    {
        var result = await _foodService.GetAllFoodsAsync();
        return Ok(result);
    }

    // PUT: api/food/{id}
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(FoodResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FoodResponseDTO>> UpdateFood(int id, [FromBody] FoodRequestDTO request)
    {
        var result = await _foodService.UpdateFoodAsync(id, request);
        return Ok(result);
    }

    // DELETE: api/food/{id}
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes. Status404NotFound)]
    public async Task<IActionResult> DeleteFood(int id)
    {
        await _foodService.RemoveFoodAsync(id);
        return NoContent();
    }
}