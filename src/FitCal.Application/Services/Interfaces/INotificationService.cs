using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Data.DTO.Response;

namespace FitCal.Application.Services.Interfaces;

// public interface INotificationService
// {
//     // Проверить калории vs дневная норма и (если надо) создать уведомление.
//     // Возвращает NotificationResponseDto если уведомление создано, иначе null.
//     Task<NotificationResponseDTO?> CheckCaloriesAndCreateNotificationAsync(int userInformationId, NotificationCheckRequestDTO check);
//
//     // Получить все уведомления пользователя
//     Task<IReadOnlyList<NotificationResponseDTO>> GetUserNotificationsAsync(int userInformationId);
//
//     // Пометить уведомление прочитанным
//     Task MarkNotificationAsReadAsync(int notificationId);
// }