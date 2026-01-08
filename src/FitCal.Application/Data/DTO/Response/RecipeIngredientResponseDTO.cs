namespace FitCal.Application.Data.DTO.Request;

public record RecipeIngredientResponseDTO(
    int FoodId,
    string FoodName,
    double Quantity,
    double Calories,
    double Protein,
    double Carbs,
    double Fats
);