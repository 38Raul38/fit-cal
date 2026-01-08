namespace FitCal.Application.Data.DTO.Response;

public record UserProfileRequestDTO(
    DateTime BirthDate,
    string Gender,
    double Height,
    double Weight,
    double WeightGoal,
    string ActivityLevel
);