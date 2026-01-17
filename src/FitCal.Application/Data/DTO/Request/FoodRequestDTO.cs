namespace FitCal.Application.Data.DTO.Response;

public sealed record FoodRequestDTO(
    string Name,           // Название продукта (например "pizza")
    double? ServingSize,    // Размер порции (например 150)
    string? ServingUnit = "g"    // Единица измерения (например "g")
);