namespace FitCal.Application.Data.DTO.Response;

public record RecipeIngredientRequestDTO(
    int FoodId,
    double Quantity
);