namespace FitCal.Application.Data.DTO.Response;

public record UserProfileResponseDTO(
    int UserInformationId,
    DateTime BirthDate,
    string Gender,
    double Height,
    double Weight,
    double WeightGoal,
    string ActivityLevel,
    double DailyCalories,
    double Protein,
    double Fats,
    double Carbs
);