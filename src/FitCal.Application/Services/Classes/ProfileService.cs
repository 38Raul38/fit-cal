using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Data.DTO.Response;
using FitCal.Application.Services.Interfaces;
using FitCal.Core.Models;
using FitCal.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FitCal.Application.Services.Classes;

public sealed class ProfileService : IProfileService
{
    private readonly FitCalContext _context;

    public ProfileService(FitCalContext context)
    {
        _context = context;
    }

    public async Task<UserProfileResponseDTO> GetProfileAsync(Guid userId)
    {
        var dto = await _context.UserInformations
            .Where(x => x.AuthUserId == userId)
            .Select(x => new UserProfileResponseDTO(
                x.UserInformationId,
                x.BirthDate,
                x.Gender,
                x.Height,
                x.Weight,
                x.WeightGoal,
                x.ActivityLevel,
                x.DailyCalories,
                x.Protein,
                x.Fats,
                x.Carbs
            ))
            .FirstOrDefaultAsync();

        if (dto == null)
            throw new KeyNotFoundException("Profile not found");

        return dto;
    }

    public async Task<UserProfileResponseDTO> SaveProfileAsync(Guid userId, UserProfileRequestDTO request)
    {
        var entity = await _context.UserInformations
            .FirstOrDefaultAsync(x => x.AuthUserId == userId);

        if (entity == null)
        {
            entity = new UserInformation
            {
                AuthUserId = userId
            };
            _context.UserInformations.Add(entity);
        }

        entity.BirthDate = request.BirthDate.ToUniversalTime();
        entity.Gender = request.Gender;
        entity.Height = request.Height;
        entity.Weight = request.Weight;
        entity.WeightGoal = request.WeightGoal;
        entity.ActivityLevel = request.ActivityLevel;

        await _context.SaveChangesAsync();

        return new UserProfileResponseDTO(
            entity.UserInformationId,
            entity.BirthDate,
            entity.Gender,
            entity.Height,
            entity.Weight,
            entity.WeightGoal,
            entity.ActivityLevel,
            entity.DailyCalories,
            entity.Protein,
            entity.Fats,
            entity.Carbs
        );
    }
    public async Task UpdateMacrosAsync(Guid userId, int calories, double protein, double fats, double carbs)
    {
        var entity = await _context.UserInformations
            .FirstOrDefaultAsync(x => x.AuthUserId == userId);

        if (entity == null)
            throw new KeyNotFoundException("Profile not found");

        entity.DailyCalories = calories;
        entity.Protein = protein;
        entity.Fats = fats;
        entity.Carbs = carbs;

        await _context.SaveChangesAsync();
    }
    public async Task UpdateCaloriesAsync(Guid userId, int dailyCalories)
    {
        var entity = await _context.UserInformations
            .FirstOrDefaultAsync(x => x.AuthUserId == userId);

        if (entity == null)
            throw new KeyNotFoundException("Profile not found");

        entity.DailyCalories = dailyCalories;

        await _context.SaveChangesAsync();
    }
}
