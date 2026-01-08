namespace FitCal.Application.Data.DTO.Request;

public record ReportResponseDTO(
    int ReportId,
    string ReportName,
    DateTime ReportDate,
    UserHistoryResponseDTO? UserHistory
);