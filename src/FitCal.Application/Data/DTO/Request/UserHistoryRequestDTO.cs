namespace FitCal.Application.Data.DTO.Response;

public record UserHistoryRequestDTO(
    DateTime JournalDate,
    int FoodId,
    int UserInformationId
);