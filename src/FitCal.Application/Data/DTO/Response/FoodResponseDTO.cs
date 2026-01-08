namespace FitCal.Application.Data.DTO.Request;

public record FoodResponseDTO(
    int FoodId,
    string Name,
    double ServingSize,
    string ServingUnit,
    double Calories,
    double Protein,
    double Carbs,
    double Fats
);