using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Data.DTO.Response;
using FitCal.Application.Services.Interfaces;
using FitCal.Core.Models;
using FitCal.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FitCal.Application.Services.Classes;

public class UserHistoryService : IUserHistoryService
{
    private readonly FitCalContext _context;

    public UserHistoryService(FitCalContext context)
    {
        _context = context;
    }

    // Добавить запись в историю
    public async Task<UserHistoryResponseDTO> AddUserHistoryRecordAsync(UserHistoryRequestDTO recordRequest)
    {
        // Валидация:  проверяем, что пользователь существует
        var userExists = await _context.UserInformations
            .AnyAsync(u => u.UserInformationId == recordRequest.UserInformationId);

        if (!userExists)
        {
            throw new KeyNotFoundException($"Пользователь с ID {recordRequest.UserInformationId} не найден");
        }

        // Валидация: проверяем, что продукт существует
        var foodExists = await _context.Foods
            .AnyAsync(f => f.FoodId == recordRequest.FoodId);

        if (!foodExists)
        {
            throw new KeyNotFoundException($"Продукт с ID {recordRequest.FoodId} не найден");
        }

        var userHistory = new UserHistory
        {
            JournalDate = recordRequest.JournalDate,
            FoodId = recordRequest.FoodId,
            UserInformationId = recordRequest.UserInformationId
        };

        _context.UserHistories.Add(userHistory);
        await _context.SaveChangesAsync();

        return await GetUserHistoryRecordByIdAsync(userHistory.UserHistoryId);
    }

    // Получить запись по ID
    public async Task<UserHistoryResponseDTO> GetUserHistoryRecordByIdAsync(int userHistoryId)
    {
        var userHistory = await _context.UserHistories
            . Include(uh => uh.Food)
            .FirstOrDefaultAsync(uh => uh.UserHistoryId == userHistoryId);

        if (userHistory == null)
        {
            throw new KeyNotFoundException($"Запись истории с ID {userHistoryId} не найдена");
        }

        return MapToResponseDto(userHistory);
    }

    // Получить все записи пользователя
    public async Task<IReadOnlyList<UserHistoryResponseDTO>> GetUserHistoryAsync(int userInformationId)
    {
        var userHistories = await _context. UserHistories
            .Where(uh => uh.UserInformationId == userInformationId)
            .Include(uh => uh.Food)
            .OrderByDescending(uh => uh.JournalDate)
            .ToListAsync();

        return userHistories.Select(MapToResponseDto).ToList();
    }

    // Получить записи за конкретную дату
    public async Task<IReadOnlyList<UserHistoryResponseDTO>> GetUserHistoryByDateAsync(int userInformationId, DateTime journalDate)
    {
        // Сравниваем только дату без времени
        var dateOnly = journalDate.Date;

        var userHistories = await _context.UserHistories
            . Where(uh => uh. UserInformationId == userInformationId 
                      && uh.JournalDate.Date == dateOnly)
            .Include(uh => uh.Food)
            .OrderBy(uh => uh.JournalDate)
            .ToListAsync();

        return userHistories.Select(MapToResponseDto).ToList();
    }

    // Удалить запись
    public async Task RemoveUserHistoryRecordAsync(int userHistoryId)
    {
        var userHistory = await _context.UserHistories. FindAsync(userHistoryId);

        if (userHistory == null)
        {
            throw new KeyNotFoundException($"Запись истории с ID {userHistoryId} не найдена");
        }

        _context.UserHistories.Remove(userHistory);
        await _context.SaveChangesAsync();
    }

    // Маппинг UserHistory -> UserHistoryResponseDTO
    private static UserHistoryResponseDTO MapToResponseDto(UserHistory userHistory)
    {
        var food = userHistory.Food;
        var foodDto = new FoodResponseDTO(
            food. FoodId,
            food. Name,
            food.ServingSize,
            food.ServingUnit,
            food.Calories,
            food.Protein,
            food.Carbs,
            food.Fats
        );

        return new UserHistoryResponseDTO(
            userHistory.UserHistoryId,
            userHistory.JournalDate,
            foodDto
        );
    }
}