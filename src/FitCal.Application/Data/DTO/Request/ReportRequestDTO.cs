namespace FitCal.Application.Data.DTO.Response;

public record ReportRequestDTO(
    string ReportName,
    DateTime ReportDate,
    int UserInformationId,
    int? UserHistoryId
);