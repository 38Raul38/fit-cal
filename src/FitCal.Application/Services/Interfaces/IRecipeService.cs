using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Data.DTO.Response;

namespace FitCal.Application.Services.Interfaces;

public interface IRecipeService
{
    // Создать рецепт (ингредиенты в граммах), вернуть рецепт с RecipeId и рассчитанными нутриентами по ингредиентам
    Task<RecipeResponseDTO> AddRecipeAsync(RecipeCreateRequestDTO recipe);

    // Получить рецепт по RecipeId (с ингредиентами и их нутриентами)
    Task<RecipeResponseDTO> GetRecipeByIdAsync(int recipeId);

    // Получить список всех рецептов
    Task<IReadOnlyList<RecipeResponseDTO>> GetAllRecipesAsync();

    // Обновить рецепт по RecipeId, вернуть обновлённый рецепт
    Task<RecipeResponseDTO> UpdateRecipeAsync(int recipeId, RecipeCreateRequestDTO recipe);

    // Удалить рецепт по RecipeId
    Task RemoveRecipeAsync(int recipeId);
}