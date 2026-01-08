namespace FitCal.Application.Data.DTO.Request;

public record NotificationResponseDTO(
    int Id,
    string Message,
    DateTime CreatedAt,
    bool IsRead
);