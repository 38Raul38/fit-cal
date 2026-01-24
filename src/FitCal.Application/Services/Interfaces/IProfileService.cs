using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Data.DTO.Response;

namespace FitCal.Application.Services.Interfaces;

public interface IProfileService
{
    Task<UserProfileResponseDTO> GetProfileAsync(Guid userId);
    Task<UserProfileResponseDTO> SaveProfileAsync(Guid userId, UserProfileRequestDTO request);
    Task UpdateCaloriesAsync(Guid userId, int dailyCalories);
}