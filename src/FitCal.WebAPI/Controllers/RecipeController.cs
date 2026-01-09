using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Data.DTO.Response;
using FitCal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FitCal.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipeController : ControllerBase
{
    private readonly IRecipeService _recipeService;

    public RecipeController(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    // POST: api/recipe
    [HttpPost]
    [ProducesResponseType(typeof(RecipeResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RecipeResponseDTO>> CreateRecipe([FromBody] RecipeCreateRequestDTO request)
    {
        var result = await _recipeService.AddRecipeAsync(request);
        return CreatedAtAction(nameof(GetRecipeById), new { id = result. RecipeId }, result);
    }

    // GET: api/recipe/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RecipeResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RecipeResponseDTO>> GetRecipeById(int id)
    {
        var result = await _recipeService.GetRecipeByIdAsync(id);
        return Ok(result);
    }

    // GET: api/recipe
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<RecipeResponseDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<RecipeResponseDTO>>> GetAllRecipes()
    {
        var result = await _recipeService.GetAllRecipesAsync();
        return Ok(result);
    }

    // PUT:  api/recipe/{id}
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(RecipeResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RecipeResponseDTO>> UpdateRecipe(int id, [FromBody] RecipeCreateRequestDTO request)
    {
        var result = await _recipeService.UpdateRecipeAsync(id, request);
        return Ok(result);
    }

    // DELETE: api/recipe/{id}
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes. Status404NotFound)]
    public async Task<IActionResult> DeleteRecipe(int id)
    {
        await _recipeService.RemoveRecipeAsync(id);
        return NoContent();
    }
}