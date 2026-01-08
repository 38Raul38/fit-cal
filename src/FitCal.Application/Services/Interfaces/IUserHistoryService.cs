using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Data.DTO.Response;

namespace FitCal.Application.Services.Interfaces;

public interface IUserHistoryService
{
    // Добавить запись в историю питания (дата + продукт + пользователь), вернуть запись с UserHistoryId и Food (как FoodResponseDTO)
    Task<UserHistoryResponseDTO> AddUserHistoryRecordAsync(UserHistoryRequestDTO record);
        
    // Получить запись истории по UserHistoryId
    Task<UserHistoryResponseDTO> GetUserHistoryRecordByIdAsync(int userHistoryId);

    // Получить все записи истории пользователя
    Task<IReadOnlyList<UserHistoryResponseDTO>> GetUserHistoryAsync(int userInformationId);

    // Получить записи истории пользователя за конкретную дату
    Task<IReadOnlyList<UserHistoryResponseDTO>> GetUserHistoryByDateAsync(int userInformationId, DateTime journalDate);

    // Удалить запись истории по UserHistoryId
    Task RemoveUserHistoryRecordAsync(int userHistoryId);
}