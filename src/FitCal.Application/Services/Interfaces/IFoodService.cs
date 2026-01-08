using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Data.DTO.Response;

namespace FitCal.Application.Services.Interfaces;

public interface IFoodService
{
    // Добавить новый продукт, вернуть продукт с FoodId
    Task<FoodResponseDTO> AddFoodAsync(FoodRequestDTO food);

    // Получить продукт по FoodId
    Task<FoodResponseDTO> GetFoodByIdAsync(int foodId);

    // Получить список всех продуктов
    Task<IReadOnlyList<FoodResponseDTO>> GetAllFoodsAsync();

    // Обновить продукт по FoodId, вернуть обновлённый продукт
    Task<FoodResponseDTO> UpdateFoodAsync(int foodId, FoodRequestDTO food);

    // Удалить продукт по FoodId
    Task RemoveFoodAsync(int foodId);
}