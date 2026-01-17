using FitCal.Application.Data.DTO.Response;

namespace FitCal.Application.Services.Interfaces;

public interface IFoodSearchService
{
    Task<FoodSearchResultDTO?> SearchFoodAsync(string query);
}