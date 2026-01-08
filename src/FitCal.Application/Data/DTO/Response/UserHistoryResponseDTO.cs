namespace FitCal.Application.Data.DTO.Request;

public record UserHistoryResponseDTO(
    int UserHistoryId,
    DateTime JournalDate,
    FoodResponseDTO Food
);