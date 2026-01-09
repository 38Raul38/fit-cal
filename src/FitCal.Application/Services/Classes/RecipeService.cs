using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Data.DTO.Response;
using FitCal.Application.Services.Interfaces;
using FitCal.Core.Models;
using FitCal.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FitCal.Application.Services.Classes;

public class RecipeService :  IRecipeService
{
    private readonly FitCalContext _context;

    public RecipeService(FitCalContext context)
    {
        _context = context;
    }

    // Создать рецепт
    public async Task<RecipeResponseDTO> AddRecipeAsync(RecipeCreateRequestDTO recipeRequest)
    {
        // Валидация:  проверяем, что все FoodId существуют
        var foodIds = recipeRequest.Ingredients. Select(i => i.FoodId).ToList();
        var existingFoodIds = await _context.Foods
            .Where(f => foodIds.Contains(f.FoodId))
            .Select(f => f.FoodId)
            .ToListAsync();

        var missingFoodIds = foodIds. Except(existingFoodIds).ToList();
        if (missingFoodIds.Any())
        {
            throw new KeyNotFoundException($"Продукты с ID {string.Join(", ", missingFoodIds)} не найдены");
        }

        var recipe = new Recipe
        {
            RecipeName = recipeRequest.RecipeName,
            ServingSize = recipeRequest.ServingSize,
            ServingUnit = recipeRequest.ServingUnit,
            RecipeIngredients = recipeRequest.Ingredients. Select(i => new RecipeIngredient
            {
                FoodId = i.FoodId,
                Quantity = i. Quantity
            }).ToList()
        };

        _context. Recipes.Add(recipe);
        await _context.SaveChangesAsync();

        // Загружаем рецепт с ингредиентами для возврата
        return await GetRecipeByIdAsync(recipe.RecipeId);
    }

    // Получить рецепт по ID
    public async Task<RecipeResponseDTO> GetRecipeByIdAsync(int recipeId)
    {
        var recipe = await _context. Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Food)
            .FirstOrDefaultAsync(r => r.RecipeId == recipeId);

        if (recipe == null)
        {
            throw new KeyNotFoundException($"Рецепт с ID {recipeId} не найден");
        }

        return MapToResponseDto(recipe);
    }

    // Получить все рецепты
    public async Task<IReadOnlyList<RecipeResponseDTO>> GetAllRecipesAsync()
    {
        var recipes = await _context.Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Food)
            .ToListAsync();

        return recipes.Select(MapToResponseDto).ToList();
    }

    // Обновить рецепт
    public async Task<RecipeResponseDTO> UpdateRecipeAsync(int recipeId, RecipeCreateRequestDTO recipeRequest)
    {
        var recipe = await _context.Recipes
            .Include(r => r. RecipeIngredients)
            .FirstOrDefaultAsync(r => r.RecipeId == recipeId);

        if (recipe == null)
        {
            throw new KeyNotFoundException($"Рецепт с ID {recipeId} не найден");
        }

        // Валидация:  проверяем, что все FoodId существуют
        var foodIds = recipeRequest. Ingredients.Select(i => i. FoodId).ToList();
        var existingFoodIds = await _context.Foods
            .Where(f => foodIds.Contains(f.FoodId))
            .Select(f => f.FoodId)
            .ToListAsync();

        var missingFoodIds = foodIds.Except(existingFoodIds).ToList();
        if (missingFoodIds.Any())
        {
            throw new KeyNotFoundException($"Продукты с ID {string.Join(", ", missingFoodIds)} не найдены");
        }

        // Обновляем основные поля
        recipe.RecipeName = recipeRequest.RecipeName;
        recipe.ServingSize = recipeRequest.ServingSize;
        recipe.ServingUnit = recipeRequest.ServingUnit;

        // Удаляем старые ингредиенты
        _context.RecipeIngredients. RemoveRange(recipe.RecipeIngredients);

        // Доб��вляем новые ингредиенты
        recipe. RecipeIngredients = recipeRequest.Ingredients.Select(i => new RecipeIngredient
        {
            RecipeId = recipeId,
            FoodId = i.FoodId,
            Quantity = i. Quantity
        }).ToList();

        await _context.SaveChangesAsync();

        // Загружаем обновлённый рецепт
        return await GetRecipeByIdAsync(recipeId);
    }

    // Удалить рецепт
    public async Task RemoveRecipeAsync(int recipeId)
    {
        var recipe = await _context.Recipes
            .Include(r => r. RecipeIngredients)
            .FirstOrDefaultAsync(r => r.RecipeId == recipeId);

        if (recipe == null)
        {
            throw new KeyNotFoundException($"Рецепт с ID {recipeId} не найден");
        }

        _context.Recipes. Remove(recipe);
        await _context.SaveChangesAsync();
    }

    // Маппинг Recipe -> RecipeResponseDTO
    private static RecipeResponseDTO MapToResponseDto(Recipe recipe)
    {
        var ingredients = recipe.RecipeIngredients.Select(ri =>
        {
            // Рассчитываем нутриенты на основе количества
            var food = ri.Food;
            var ratio = ri.Quantity / food.ServingSize;

            return new RecipeIngredientResponseDTO(
                food.FoodId,
                food.Name,
                ri.Quantity,
                food.Calories * ratio,
                food.Protein * ratio,
                food. Carbs * ratio,
                food.Fats * ratio
            );
        }).ToList();

        return new RecipeResponseDTO(
            recipe.RecipeId,
            recipe.RecipeName,
            recipe.ServingSize,
            recipe.ServingUnit,
            ingredients
        );
    }
}