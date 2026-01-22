namespace FitCal.Application.Data.DTO.Request;

public record UserHistoryRequestDTO(
    DateTime JournalDate,
    int FoodId
);