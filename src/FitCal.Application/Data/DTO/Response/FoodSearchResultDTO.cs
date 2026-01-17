namespace FitCal.Application.Data. DTO.Response;

public sealed record FoodSearchResultDTO(
    string FoodName,
    double ServingSize,
    string ServingUnit,
    double Calories,
    double Protein,
    double Carbs,
    double Fats
);