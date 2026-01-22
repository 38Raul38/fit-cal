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

    public async Task<UserHistoryResponseDTO> AddUserHistoryRecordAsync(Guid authUserId, UserHistoryRequestDTO record)
    {
        var userInfoId = await _context.UserInformations
            .Where(x => x.AuthUserId == authUserId)
            .Select(x => x.UserInformationId)
            .FirstOrDefaultAsync();

        if (userInfoId == 0)
            throw new KeyNotFoundException("UserInformation для текущего пользователя не найден");

        var foodExists = await _context.Foods.AnyAsync(f => f.FoodId == record.FoodId);
        if (!foodExists)
            throw new KeyNotFoundException($"Продукт с ID {record.FoodId} не найден");

        var entity = new UserHistory
        {
            JournalDate = record.JournalDate,
            FoodId = record.FoodId,
            UserInformationId = userInfoId
        };

        _context.UserHistories.Add(entity);
        await _context.SaveChangesAsync();

        return await GetUserHistoryRecordByIdAsync(entity.UserHistoryId);
    }

    public async Task<UserHistoryResponseDTO> GetUserHistoryRecordByIdAsync(int userHistoryId)
    {
        var entity = await _context.UserHistories
            .Include(x => x.Food)
            .FirstOrDefaultAsync(x => x.UserHistoryId == userHistoryId);

        if (entity == null)
            throw new KeyNotFoundException($"Запись истории с ID {userHistoryId} не найдена");

        return MapToResponseDto(entity);
    }

    public async Task<IReadOnlyList<UserHistoryResponseDTO>> GetUserHistoryAsync(Guid authUserId)
    {
        var userInfoId = await _context.UserInformations
            .Where(x => x.AuthUserId == authUserId)
            .Select(x => x.UserInformationId)
            .FirstOrDefaultAsync();

        if (userInfoId == 0)
            return [];

        var list = await _context.UserHistories
            .Where(x => x.UserInformationId == userInfoId)
            .Include(x => x.Food)
            .OrderByDescending(x => x.JournalDate)
            .ToListAsync();

        return list.Select(MapToResponseDto).ToList();
    }

    public async Task<IReadOnlyList<UserHistoryResponseDTO>> GetUserHistoryByDateAsync(Guid authUserId, DateTime journalDate)
    {
        var userInfoId = await _context.UserInformations
            .Where(x => x.AuthUserId == authUserId)
            .Select(x => x.UserInformationId)
            .FirstOrDefaultAsync();

        if (userInfoId == 0)
            return [];

        var dateOnly = journalDate.Date;

        var list = await _context.UserHistories
            .Where(x => x.UserInformationId == userInfoId && x.JournalDate.Date == dateOnly)
            .Include(x => x.Food)
            .OrderBy(x => x.JournalDate)
            .ToListAsync();

        return list.Select(MapToResponseDto).ToList();
    }

    public async Task RemoveUserHistoryRecordAsync(Guid authUserId, int userHistoryId)
    {
        var userInfoId = await _context.UserInformations
            .Where(x => x.AuthUserId == authUserId)
            .Select(x => x.UserInformationId)
            .FirstOrDefaultAsync();

        if (userInfoId == 0)
            throw new KeyNotFoundException("UserInformation для текущего пользователя не найден");

        var entity = await _context.UserHistories
            .FirstOrDefaultAsync(x => x.UserHistoryId == userHistoryId && x.UserInformationId == userInfoId);

        if (entity == null)
            throw new KeyNotFoundException($"Запись истории с ID {userHistoryId} не найдена");

        _context.UserHistories.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static UserHistoryResponseDTO MapToResponseDto(UserHistory userHistory)
    {
        var food = userHistory.Food;
        var foodDto = new FoodResponseDTO(
            food.FoodId,
            food.Name,
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
