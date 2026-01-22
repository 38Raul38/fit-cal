using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Data.DTO.Response;
using FitCal.Application.Services.Interfaces;
using FitCal.Core.Models;
using FitCal.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FitCal.Application.Services.Classes;

public sealed class FoodService : IFoodService
{
    private readonly FitCalContext _context;
    private readonly IFoodSearchService _foodSearchService;

    public FoodService(FitCalContext context, IFoodSearchService foodSearchService)
    {
        _context = context;
        _foodSearchService = foodSearchService;
    }

    public async Task<FoodResponseDTO> AddFoodAsync(Guid authUserId, FoodRequestDTO foodRequest)
    {
        var apiFood = await _foodSearchService.SearchFoodAsync(foodRequest.Name);

        if (apiFood == null)
            throw new KeyNotFoundException($"Продукт '{foodRequest.Name}' не найден в базе питания");

        double servingSize = foodRequest.ServingSize ?? apiFood.ServingSize;
        string servingUnit = foodRequest.ServingUnit ?? "g";

        double ratio = servingSize / apiFood.ServingSize;

        var food = new Food
        {
            Name = apiFood.FoodName,
            ServingSize = servingSize,
            ServingUnit = servingUnit,
            Calories = apiFood.Calories * ratio,
            Protein = apiFood.Protein * ratio,
            Carbs = apiFood.Carbs * ratio,
            Fats = apiFood.Fats * ratio,
            OwnerAuthUserId = authUserId
        };

        _context.Foods.Add(food);
        await _context.SaveChangesAsync();

        return MapToResponseDto(food);
    }

    public async Task<FoodResponseDTO> GetFoodByIdAsync(Guid authUserId, int foodId)
    {
        var food = await _context.Foods
            .FirstOrDefaultAsync(x => x.FoodId == foodId && x.OwnerAuthUserId == authUserId);

        if (food == null)
            throw new KeyNotFoundException($"Продукт с ID {foodId} не найден");

        return MapToResponseDto(food);
    }

    public async Task<IReadOnlyList<FoodResponseDTO>> GetAllFoodsAsync(Guid authUserId)
    {
        var foods = await _context.Foods
            .Where(x => x.OwnerAuthUserId == authUserId)
            .OrderByDescending(x => x.FoodId)
            .ToListAsync();

        return foods.Select(MapToResponseDto).ToList();
    }

    public async Task<FoodResponseDTO> UpdateFoodAsync(Guid authUserId, int foodId, FoodRequestDTO foodRequest)
    {
        var food = await _context.Foods
            .FirstOrDefaultAsync(x => x.FoodId == foodId && x.OwnerAuthUserId == authUserId);

        if (food == null)
            throw new KeyNotFoundException($"Продукт с ID {foodId} не найден");

        var apiFood = await _foodSearchService.SearchFoodAsync(foodRequest.Name);

        if (apiFood == null)
            throw new KeyNotFoundException($"Продукт '{foodRequest.Name}' не найден в базе питания");

        double servingSize = foodRequest.ServingSize ?? apiFood.ServingSize;
        string servingUnit = foodRequest.ServingUnit ?? "g";
        double ratio = servingSize / apiFood.ServingSize;

        food.Name = apiFood.FoodName;
        food.ServingSize = servingSize;
        food.ServingUnit = servingUnit;
        food.Calories = apiFood.Calories * ratio;
        food.Protein = apiFood.Protein * ratio;
        food.Carbs = apiFood.Carbs * ratio;
        food.Fats = apiFood.Fats * ratio;

        await _context.SaveChangesAsync();

        return MapToResponseDto(food);
    }

    public async Task RemoveFoodAsync(Guid authUserId, int foodId)
    {
        var food = await _context.Foods
            .FirstOrDefaultAsync(x => x.FoodId == foodId && x.OwnerAuthUserId == authUserId);

        if (food == null)
            throw new KeyNotFoundException($"Продукт с ID {foodId} не найден");

        _context.Foods.Remove(food);
        await _context.SaveChangesAsync();
    }

    private static FoodResponseDTO MapToResponseDto(Food food)
        => new(
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
