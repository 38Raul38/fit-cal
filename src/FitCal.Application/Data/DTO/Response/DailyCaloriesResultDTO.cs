namespace FitCal.Application.Data.DTO.Request;

public sealed record DailyCaloriesResultDTO(
    int DailyCalories,
    string UnitLabel,
    string PlanText,
    GoalType Goal
);