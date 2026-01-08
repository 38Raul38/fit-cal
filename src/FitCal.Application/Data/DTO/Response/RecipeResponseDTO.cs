namespace FitCal.Application.Data.DTO.Request;

public record RecipeResponseDTO(
    int RecipeId,
    string RecipeName,
    double ServingSize,
    string ServingUnit,
    List<RecipeIngredientResponseDTO> Ingredients
);