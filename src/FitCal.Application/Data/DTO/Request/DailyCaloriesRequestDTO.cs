namespace FitCal.Application.Data.DTO.Request;

public enum GoalType
{
    LoseWeight = 0,
    MaintainWeight = 1,
    GainWeight = 2
}

public enum ActivityLevel
{
    NotVeryActive = 0,
    LightlyActive = 1,
    Active = 2,
    VeryActive = 3
}

public enum Gender
{
    Male = 0,
    Female = 1
}

public sealed record DailyCaloriesRequestDTO(
    GoalType Goal,
    ActivityLevel ActivityLevel,
    Gender Gender,
    DateOnly BirthDate,
    double HeightCm,
    double WeightKg,
    double?  GoalWeightKg // желаемый вес (null для Maintain)
);