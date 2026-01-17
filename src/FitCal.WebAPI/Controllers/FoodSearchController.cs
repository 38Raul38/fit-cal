using FitCal.Application.Data.DTO.Response;
using FitCal.Application.Services. Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FitCal.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FoodSearchController : ControllerBase
{
    private readonly IFoodSearchService _foodSearchService;

    public FoodSearchController(IFoodSearchService foodSearchService)
    {
        _foodSearchService = foodSearchService;
    }

    /// <summary>
    /// Найти продукт по названию (возвращает КБЖУ из CalorieNinjas API)
    /// </summary>
    /// <param name="query">Название продукта (например:  "egg", "pizza", "chicken")</param>
    /// <returns>КБЖУ продукта для стандартной порции</returns>
    /// <response code="200">Продукт найден</response>
    /// <response code="404">Продукт не найден в базе CalorieNinjas</response>
    /// <response code="400">Некорректный запрос (пустое название)</response>
    [HttpGet]
    [ProducesResponseType(typeof(FoodSearchResultDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchFood([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest(new { message = "Параметр 'query' обязателен и не может быть пустым" });
        }

        Console.WriteLine($"🔍 [FoodSearchController] Поиск:  '{query}'");

        var result = await _foodSearchService. SearchFoodAsync(query);

        if (result == null)
        {
            Console.WriteLine($"❌ [FoodSearchController] Продукт '{query}' не найден");
            return NotFound(new { message = $"Продукт '{query}' не найден в базе питания" });
        }

        Console.WriteLine($"✅ [FoodSearchController] Найдено: {result.FoodName}, {result. Calories} ккал");

        return Ok(result);
    }
}