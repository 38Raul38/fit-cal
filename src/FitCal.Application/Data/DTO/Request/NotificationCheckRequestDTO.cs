namespace FitCal.Application.Data.DTO.Response;

public record NotificationCheckRequestDTO(
    double CurrentCalories,
    double DailyCalories
);