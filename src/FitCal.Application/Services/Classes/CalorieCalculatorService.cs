using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Services.Interfaces;

namespace FitCal.Application.Services.Classes;

public sealed class CalorieCalculatorService : ICalorieCalculatorService
{
    private const double KcalPerKg = 7700.0;

    public Task<DailyCaloriesResultDTO> CalculateDailyCaloriesAsync(DailyCaloriesRequestDTO request)
    {
        Validate(request);

        int age = CalculateAge(request.BirthDate, DateOnly.FromDateTime(DateTime.UtcNow));
        double activityFactor = GetActivityFactor(request.ActivityLevel);

        double bmr = CalculateBmr(request.Gender, request.WeightKg, request.HeightCm, age);
        double tdee = bmr * activityFactor;

        double delta = CalculateGoalDeltaPerDay(request.Goal, request.TargetRateKgPerWeek);
        double rawDaily = tdee + delta;

        double minCalories = request.Gender == Gender.Female ? 1200 : 1500;
        rawDaily = Math.Max(rawDaily, minCalories);

        int dailyRounded = (int)(Math.Round(rawDaily / 10.0) * 10);

        var result = new DailyCaloriesResultDTO(
            DailyCalories: dailyRounded,
            UnitLabel: "калории",
            PlanText: GetPlanText(request.Goal),
            Goal: request.Goal
        );

        return Task.FromResult(result);
    }

    private static string GetPlanText(GoalType goal) =>
        goal switch
        {
            GoalType.MaintainWeight => "Поддерживать свой текущий вес",
            GoalType.LoseWeight => "Снижать вес",
            GoalType.GainWeight => "Набирать вес",
            _ => "План питания"
        };

    private static void Validate(DailyCaloriesRequestDTO r)
    {
        if (r.HeightCm is < 50 or > 260)
            throw new ArgumentOutOfRangeException(nameof(r.HeightCm), "HeightCm должен быть в диапазоне 50..260.");

        if (r.WeightKg is < 20 or > 500)
            throw new ArgumentOutOfRangeException(nameof(r.WeightKg), "WeightKg должен быть в диапазоне 20..500.");

        if (r.Goal != GoalType.MaintainWeight)
        {
            if (r.TargetRateKgPerWeek is null)
                throw new ArgumentException("Для LoseWeight/GainWeight нужно указать TargetRateKgPerWeek.", nameof(r.TargetRateKgPerWeek));

            if (r.TargetRateKgPerWeek <= 0 || r.TargetRateKgPerWeek > 1.5)
                throw new ArgumentOutOfRangeException(nameof(r.TargetRateKgPerWeek), "TargetRateKgPerWeek должен быть > 0 и <= 1.5.");
        }
    }

    private static int CalculateAge(DateOnly birthDate, DateOnly today)
    {
        int age = today.Year - birthDate.Year;
        if (today < birthDate.AddYears(age)) age--;
        return Math.Max(age, 0);
    }

    private static double GetActivityFactor(ActivityLevel level) =>
        level switch
        {
            ActivityLevel.NotVeryActive => 1.2,
            ActivityLevel.LightlyActive => 1.375,
            ActivityLevel.Active => 1.55,
            ActivityLevel.VeryActive => 1.725,
            _ => throw new ArgumentOutOfRangeException(nameof(level))
        };

    private static double CalculateBmr(Gender gender, double weightKg, double heightCm, int age)
    {
        double baseValue = (10.0 * weightKg) + (6.25 * heightCm) - (5.0 * age);
        return gender switch
        {
            Gender.Male => baseValue + 5.0,
            Gender.Female => baseValue - 161.0,
            _ => throw new ArgumentOutOfRangeException(nameof(gender))
        };
    }

    private static double CalculateGoalDeltaPerDay(GoalType goal, double? rateKgPerWeek)
    {
        if (goal == GoalType.MaintainWeight) return 0.0;

        double deltaPerDay = (rateKgPerWeek!.Value * KcalPerKg) / 7.0;

        return goal switch
        {
            GoalType.LoseWeight => -deltaPerDay,
            GoalType.GainWeight => +deltaPerDay,
            _ => throw new ArgumentOutOfRangeException(nameof(goal))
        };
    }
}