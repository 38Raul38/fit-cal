using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Data.DTO.Response;
using FitCal.Application.Services.Interfaces;
using FitCal.Core.Models;
using FitCal.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FitCal.Application.Services.Classes;

public class ReportService : IReportService
{
    private readonly FitCalContext _context;

    public ReportService(FitCalContext context)
    {
        _context = context;
    }

    // Создать отчёт
    public async Task<ReportResponseDTO> AddReportAsync(ReportRequestDTO reportRequest)
    {
        // Валидация:  проверяем, что пользователь существует
        var userExists = await _context.UserInformations
            .AnyAsync(u => u.UserInformationId == reportRequest.UserInformationId);

        if (!userExists)
        {
            throw new KeyNotFoundException($"Пользователь с ID {reportRequest.UserInformationId} не найден");
        }

        // Валидация: если указан UserHistoryId, проверяем что он существует и принадлежит пользователю
        if (reportRequest.UserHistoryId.HasValue)
        {
            var userHistory = await _context.UserHistories
                .FirstOrDefaultAsync(uh => uh.UserHistoryId == reportRequest.UserHistoryId. Value);

            if (userHistory == null)
            {
                throw new KeyNotFoundException($"История с ID {reportRequest.UserHistoryId. Value} не найдена");
            }

            if (userHistory.UserInformationId != reportRequest.UserInformationId)
            {
                throw new InvalidOperationException($"История с ID {reportRequest.UserHistoryId.Value} не принадлежит пользователю {reportRequest.UserInformationId}");
            }
        }

        var report = new Report
        {
            ReportName = reportRequest. ReportName,
            ReportDate = reportRequest.ReportDate,
            UserInformationId = reportRequest.UserInformationId,
            UserHistoryId = reportRequest.UserHistoryId
        };

        _context.Reports.Add(report);
        await _context.SaveChangesAsync();

        return await GetReportByIdAsync(report.ReportId);
    }

    // Получить отчёт по ID
    public async Task<ReportResponseDTO> GetReportByIdAsync(int reportId)
    {
        var report = await _context.Reports
            .Include(r => r. UserHistory)
                .ThenInclude(uh => uh.Food)
            .FirstOrDefaultAsync(r => r.ReportId == reportId);

        if (report == null)
        {
            throw new KeyNotFoundException($"Отчёт с ID {reportId} не найден");
        }

        return MapToResponseDto(report);
    }

    // Получить все отчёты пользователя
    public async Task<IReadOnlyList<ReportResponseDTO>> GetUserReportsAsync(int userInformationId)
    {
        var reports = await _context.Reports
            .Where(r => r.UserInformationId == userInformationId)
            .Include(r => r.UserHistory)
                .ThenInclude(uh => uh.Food)
            .ToListAsync();

        return reports.Select(MapToResponseDto).ToList();
    }

    // Удалить отчёт
    public async Task RemoveReportAsync(int reportId)
    {
        var report = await _context.Reports.FindAsync(reportId);

        if (report == null)
        {
            throw new KeyNotFoundException($"Отчёт с ID {reportId} не найден");
        }

        _context.Reports.Remove(report);
        await _context.SaveChangesAsync();
    }

    // Маппинг Report -> ReportResponseDTO
    private static ReportResponseDTO MapToResponseDto(Report report)
    {
        UserHistoryResponseDTO? userHistoryDto = null;

        if (report.UserHistory != null)
        {
            var food = report.UserHistory.Food;
            var foodDto = new FoodResponseDTO(
                food.FoodId,
                food.Name,
                food.ServingSize,
                food.ServingUnit,
                food. Calories,
                food.Protein,
                food. Carbs,
                food. Fats
            );

            userHistoryDto = new UserHistoryResponseDTO(
                report.UserHistory.UserHistoryId,
                report. UserHistory.JournalDate,
                foodDto
            );
        }

        return new ReportResponseDTO(
            report.ReportId,
            report.ReportName,
            report.ReportDate,
            userHistoryDto
        );
    }
}