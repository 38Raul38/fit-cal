using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Data.DTO.Response;
using FitCal.Application.Services.Interfaces;
using FitCal.Core.Models;
using FitCal.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FitCal.Application.Services.Classes;



public class FoodService : IFoodService
{
    private readonly FitCalContext _context;

    public FoodService(FitCalContext context)
    {
        _context = context;
    }

    // Добавить новый продукт
    public async Task<FoodResponseDTO> AddFoodAsync(FoodRequestDTO foodRequest)
    {
        var food = new Food
        {
            Name = foodRequest.Name,
            ServingSize = foodRequest.ServingSize,
            ServingUnit = foodRequest.ServingUnit,
            Calories = foodRequest.Calories,
            Protein = foodRequest.Protein,
            Carbs = foodRequest.Carbs,
            Fats = foodRequest.Fats
        };

        _context.Foods.Add(food);
        await _context.SaveChangesAsync();

        return MapToResponseDto(food);
    }

    // Получить продукт по ID
    public async Task<FoodResponseDTO> GetFoodByIdAsync(int foodId)
    {
        var food = await _context.Foods.FindAsync(foodId);

        if (food == null)
        {
            throw new KeyNotFoundException($"Продукт с ID {foodId} не найден");
        }

        return MapToResponseDto(food);
    }

    // Получить все продукты
    public async Task<IReadOnlyList<FoodResponseDTO>> GetAllFoodsAsync()
    {
        var foods = await _context.Foods.ToListAsync();
        return foods.Select(MapToResponseDto).ToList();
    }

    // Обновить продукт
    public async Task<FoodResponseDTO> UpdateFoodAsync(int foodId, FoodRequestDTO foodRequest)
    {
        var food = await _context.Foods.FindAsync(foodId);

        if (food == null)
        {
            throw new KeyNotFoundException($"Продукт с ID {foodId} не найден");
        }

        // Обновляем поля
        food.Name = foodRequest.Name;
        food.ServingSize = foodRequest.ServingSize;
        food.ServingUnit = foodRequest.ServingUnit;
        food.Calories = foodRequest.Calories;
        food.Protein = foodRequest.Protein;
        food.Carbs = foodRequest.Carbs;
        food.Fats = foodRequest.Fats;

        _context.Foods.Update(food);
        await _context.SaveChangesAsync();

        return MapToResponseDto(food);
    }

    // Удалить продукт
    public async Task RemoveFoodAsync(int foodId)
    {
        var food = await _context.Foods.FindAsync(foodId);

        if (food == null)
        {
            throw new KeyNotFoundException($"Продукт с ID {foodId} не найден");
        }

        _context.Foods.Remove(food);
        await _context.SaveChangesAsync();
    }

    // Маппинг Entity -> DTO
    private static FoodResponseDTO MapToResponseDto(Food food)
    {
        return new FoodResponseDTO(
            food.FoodId,
            food.Name,
            food.ServingSize,
            food.ServingUnit,
            food.Calories,
            food.Protein,
            food.Carbs,
            food.Fats
        );
    }
}