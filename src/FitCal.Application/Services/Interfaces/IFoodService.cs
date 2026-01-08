using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Data.DTO.Response;

namespace FitCal.Application.Services.Interfaces;

public interface IFoodService
{
    // Добавить новый продукт, вернуть продукт с FoodId
    Task<FoodResponseDTO> AddFoodAsync(
        FoodRequestDTO food,
        CancellationToken ct = default);

    // Получить продукт по FoodId
    Task<FoodResponseDTO> GetFoodByIdAsync(
        int foodId,
        CancellationToken ct = default);

    // Получить список всех продуктов
    Task<IReadOnlyList<FoodResponseDTO>> GetAllFoodsAsync(
        CancellationToken ct = default);

    // Обновить продукт по FoodId, вернуть обновлённый продукт
    Task<FoodResponseDTO> UpdateFoodAsync(
        int foodId,
        FoodRequestDTO food,
        CancellationToken ct = default);

    // Удалить продукт по FoodId
    Task RemoveFoodAsync(
        int foodId,
        CancellationToken ct = default);
}