using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Data.DTO.Response;

namespace FitCal.Application.Services.Interfaces;

public interface IReportService
{
    // Создать отчёт (может быть привязан к UserHistoryId), вернуть отчёт с ReportId и (опционально) UserHistory
    Task<ReportResponseDTO> AddReportAsync(
        ReportRequestDTO report,
        CancellationToken ct = default);

    // Получить отчёт по ReportId
    Task<ReportResponseDTO> GetReportByIdAsync(
        int reportId,
        CancellationToken ct = default);

    // Получить все отчёты пользователя
    Task<IReadOnlyList<ReportResponseDTO>> GetUserReportsAsync(
        int userInformationId,
        CancellationToken ct = default);

    // Удалить отчёт по ReportId
    Task RemoveReportAsync(
        int reportId,
        CancellationToken ct = default);
}