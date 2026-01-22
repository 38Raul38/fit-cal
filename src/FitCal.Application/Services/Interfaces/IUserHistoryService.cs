using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Data.DTO.Response;

namespace FitCal.Application.Services.Interfaces;

public interface IUserHistoryService
{
    Task<UserHistoryResponseDTO> AddUserHistoryRecordAsync(Guid authUserId, UserHistoryRequestDTO record);

    Task<UserHistoryResponseDTO> GetUserHistoryRecordByIdAsync(int userHistoryId);

    Task<IReadOnlyList<UserHistoryResponseDTO>> GetUserHistoryAsync(Guid authUserId);

    Task<IReadOnlyList<UserHistoryResponseDTO>> GetUserHistoryByDateAsync(Guid authUserId, DateTime journalDate);

    Task RemoveUserHistoryRecordAsync(Guid authUserId, int userHistoryId);
}