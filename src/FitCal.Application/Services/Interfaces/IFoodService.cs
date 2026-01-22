using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Data.DTO.Response;

public interface IFoodService
{
    Task<FoodResponseDTO> AddFoodAsync(Guid authUserId, FoodRequestDTO food);

    Task<FoodResponseDTO> GetFoodByIdAsync(Guid authUserId, int foodId);

    Task<IReadOnlyList<FoodResponseDTO>> GetAllFoodsAsync(Guid authUserId);

    Task<FoodResponseDTO> UpdateFoodAsync(Guid authUserId, int foodId, FoodRequestDTO food);

    Task RemoveFoodAsync(Guid authUserId, int foodId);
}