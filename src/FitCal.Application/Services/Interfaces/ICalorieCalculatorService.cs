using FitCal.Application.Data.DTO.Request;

namespace FitCal.Application.Services.Interfaces;

public interface ICalorieCalculatorService
{
    Task<DailyCaloriesResultDTO> CalculateDailyCaloriesAsync(DailyCaloriesRequestDTO request);
}