namespace FitCal.Application.Data.DTO.Response;

public record FoodRequestDTO(
    string Name,
    double ServingSize,
    string ServingUnit,
    double Calories,
    double Protein,
    double Carbs,
    double Fats
);