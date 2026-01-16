using FitCal.Application.Data. DTO.Request;
using FitCal.Application.Services.Interfaces;

namespace FitCal.Application.Services.Classes;

public sealed class CalorieCalculatorService : ICalorieCalculatorService
{
    private const double KcalPerKg = 7700.0;
    
    // Дефолтные темпы (кг/неделю) — можешь настроить под себя
    private const double DefaultLoseRateKgPerWeek = 0.5;
    private const double DefaultGainRateKgPerWeek = 0.25;

    public Task<DailyCaloriesResultDTO> CalculateDailyCaloriesAsync(DailyCaloriesRequestDTO request)
    {
        Validate(request);

        int age = CalculateAge(request. BirthDate, DateOnly.FromDateTime(DateTime. UtcNow));
        double activityFactor = GetActivityFactor(request.ActivityLevel);

        double bmr = CalculateBmr(request.Gender, request.WeightKg, request.HeightCm, age);
        double tdee = bmr * activityFactor;

        double delta = CalculateGoalDeltaPerDay(request.Goal, request.WeightKg, request.GoalWeightKg);

        double rawDaily = tdee + delta;

        // минимальные безопасные значения
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
            throw new ArgumentOutOfRangeException(nameof(r.HeightCm), "Рост должен быть в диапазоне 50.. 260 см.");

        if (r.WeightKg is < 20 or > 500)
            throw new ArgumentOutOfRangeException(nameof(r.WeightKg), "Вес должен быть в диапазоне 20.. 500 кг.");

        if (r.Goal != GoalType.MaintainWeight)
        {
            if (r.GoalWeightKg is null)
                throw new ArgumentException("Для изменения веса нужно указать желаемый вес (GoalWeightKg).", nameof(r.GoalWeightKg));

            if (r.GoalWeightKg is < 20 or > 500)
                throw new ArgumentOutOfRangeException(nameof(r.GoalWeightKg), "Желаемый вес должен быть в диапазоне 20..500 кг.");

            // логическая проверка направления
            if (r.Goal == GoalType.LoseWeight && r.GoalWeightKg >= r.WeightKg)
                throw new ArgumentException("Для похудения желаемый вес должен быть меньше текущего.", nameof(r.GoalWeightKg));

            if (r.Goal == GoalType. GainWeight && r.GoalWeightKg <= r.WeightKg)
                throw new ArgumentException("Для набора веса желаемый вес должен быть больше текущего.", nameof(r.GoalWeightKg));
        }
    }

    private static int CalculateAge(DateOnly birthDate, DateOnly today)
    {
        int age = today. Year - birthDate.Year;
        if (today < birthDate. AddYears(age)) age--;
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
            Gender. Female => baseValue - 161.0,
            _ => throw new ArgumentOutOfRangeException(nameof(gender))
        };
    }

    private static double CalculateGoalDeltaPerDay(GoalType goal, double currentWeightKg, double?  goalWeightKg)
    {
        if (goal == GoalType.MaintainWeight)
            return 0.0;

        // используем дефолтный темп
        double rateKgPerWeek = goal switch
        {
            GoalType.LoseWeight => DefaultLoseRateKgPerWeek,
            GoalType.GainWeight => DefaultGainRateKgPerWeek,
            _ => throw new ArgumentOutOfRangeException(nameof(goal))
        };

        // дневное изменение калорий
        double deltaPerDay = (rateKgPerWeek * KcalPerKg) / 7.0;

        return goal switch
        {
            GoalType.LoseWeight => -deltaPerDay,
            GoalType.GainWeight => +deltaPerDay,
            _ => throw new ArgumentOutOfRangeException(nameof(goal))
        };
    }
}