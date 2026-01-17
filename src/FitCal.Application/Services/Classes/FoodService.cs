using FitCal.Application.Data. DTO. Request;
using FitCal.Application.Data.DTO. Response;
using FitCal.Application.Services.Interfaces;
using FitCal.Core.Models;
using FitCal. Persistence. Context;
using Microsoft.EntityFrameworkCore;

namespace FitCal.Application.Services. Classes;

/// <summary>
/// Сервис для работы с продуктами в базе данных
/// </summary>
public sealed class FoodService : IFoodService
{
    private readonly FitCalContext _context;
    private readonly IFoodSearchService _foodSearchService;

    public FoodService(FitCalContext context, IFoodSearchService foodSearchService)
    {
        _context = context;
        _foodSearchService = foodSearchService;
    }

    /// <summary>
    /// Добавить новый продукт (бэк САМ получает КБЖУ из CalorieNinjas API)
    /// </summary>
    public async Task<FoodResponseDTO> AddFoodAsync(FoodRequestDTO foodRequest)
    {
        Console.WriteLine($"🍽️ [AddFood] Запрос: Name={foodRequest.Name}, Size={foodRequest.ServingSize}, Unit={foodRequest.ServingUnit}");
        
        // 1. Идём в CalorieNinjas API за КБЖУ
        var apiFood = await _foodSearchService.SearchFoodAsync(foodRequest.Name);

        if (apiFood == null)
        {
            Console.WriteLine($"❌ [AddFood] Продукт не найден в CalorieNinjas");
            throw new KeyNotFoundException($"Продукт '{foodRequest. Name}' не найден в базе питания");
        }

        Console.WriteLine($"📊 [AddFood] API вернул: {apiFood.FoodName}, {apiFood. Calories} ккал, {apiFood.ServingSize}g");

        // 2. Используем порцию из запроса ИЛИ из API (если не указана)
        double servingSize = foodRequest.ServingSize ?? apiFood.ServingSize;
        string servingUnit = foodRequest.ServingUnit ?? "g";

        // 3. Пересчитываем КБЖУ под нужную порцию
        double ratio = servingSize / apiFood.ServingSize;

        Console.WriteLine($"🔢 [AddFood] Порция: {servingSize}{servingUnit}, коэффициент: {ratio}");

        var food = new Food
        {
            Name = apiFood.FoodName,
            ServingSize = servingSize,           // ✅ double, не nullable
            ServingUnit = servingUnit,           // ✅ string, не nullable
            Calories = apiFood.Calories * ratio,
            Protein = apiFood.Protein * ratio,
            Carbs = apiFood. Carbs * ratio,
            Fats = apiFood. Fats * ratio
        };

        // 4. Сохраняем в базу данных
        Console.WriteLine($"💾 [AddFood] Сохраняем в БД.. .");
        _context.Foods. Add(food);
        await _context.SaveChangesAsync();

        Console.WriteLine($"✅ [AddFood] Успешно!");
        return MapToResponseDto(food);
    }

    public async Task<FoodResponseDTO> GetFoodByIdAsync(int foodId)
    {
        var food = await _context.Foods.FindAsync(foodId);

        if (food == null)
            throw new KeyNotFoundException($"Продукт с ID {foodId} не найден");

        return MapToResponseDto(food);
    }

    public async Task<IReadOnlyList<FoodResponseDTO>> GetAllFoodsAsync()
    {
        var foods = await _context.Foods. ToListAsync();
        return foods. Select(MapToResponseDto).ToList();
    }

    public async Task<FoodResponseDTO> UpdateFoodAsync(int foodId, FoodRequestDTO foodRequest)
    {
        var food = await _context.Foods.FindAsync(foodId);

        if (food == null)
            throw new KeyNotFoundException($"Продукт с ID {foodId} не найден");

        // Получаем обновлённые данные из API
        var apiFood = await _foodSearchService.SearchFoodAsync(foodRequest.Name);

        if (apiFood == null)
            throw new KeyNotFoundException($"Продукт '{foodRequest. Name}' не найден в базе питания");

        // Используем порцию из запроса ИЛИ из API
        double servingSize = foodRequest.ServingSize ?? apiFood.ServingSize;
        string servingUnit = foodRequest.ServingUnit ?? "g";
        
        double ratio = servingSize / apiFood.ServingSize;

        food.Name = apiFood. FoodName;
        food. ServingSize = servingSize;          // ✅ double, не nullable
        food.ServingUnit = servingUnit;          // ✅ string, не nullable
        food.Calories = apiFood.Calories * ratio;
        food.Protein = apiFood. Protein * ratio;
        food.Carbs = apiFood. Carbs * ratio;
        food.Fats = apiFood.Fats * ratio;

        _context.Foods.Update(food);
        await _context. SaveChangesAsync();

        return MapToResponseDto(food);
    }

    public async Task RemoveFoodAsync(int foodId)
    {
        var food = await _context. Foods.FindAsync(foodId);

        if (food == null)
            throw new KeyNotFoundException($"Продукт с ID {foodId} не найден");

        _context.Foods.Remove(food);
        await _context.SaveChangesAsync();
    }

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