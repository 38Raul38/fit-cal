using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Data.DTO.Response;

namespace FitCal.Application.Services.Interfaces;

public interface IRecipeService
{
    // Создать рецепт (ингредиенты в граммах), вернуть рецепт с RecipeId и рассчитанными нутриентами по ингредиентам
    Task<RecipeResponseDTO> AddRecipeAsync(
        RecipeCreateRequestDTO recipe,
        CancellationToken ct = default);

    // Получить рецепт по RecipeId (с ингредиентами и их нутриентами)
    Task<RecipeResponseDTO> GetRecipeByIdAsync(
        int recipeId,
        CancellationToken ct = default);

    // Получить список всех рецептов
    Task<IReadOnlyList<RecipeResponseDTO>> GetAllRecipesAsync(
        CancellationToken ct = default);

    // Обновить рецепт по RecipeId, вернуть обновлённый рецепт
    Task<RecipeResponseDTO> UpdateRecipeAsync(
        int recipeId,
        RecipeCreateRequestDTO recipe,
        CancellationToken ct = default);

    // Удалить рецепт по RecipeId
    Task RemoveRecipeAsync(
        int recipeId,
        CancellationToken ct = default);
}