namespace FitCal.Application.Data.DTO.Response;

public record RecipeCreateRequestDTO(
    string RecipeName,
    double ServingSize,
    string ServingUnit,
    List<RecipeIngredientRequestDTO> Ingredients
);