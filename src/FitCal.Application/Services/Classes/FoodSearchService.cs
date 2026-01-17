using System.Net.Http.Json;
using System.Text. Json. Serialization;
using FitCal.Application.Data. DTO.Response;
using FitCal.Application.Services. Interfaces;
using Microsoft.Extensions.Configuration;

namespace FitCal. Application.Services.Classes;

/// <summary>
/// Сервис для получения КБЖУ продуктов через CalorieNinjas API
/// </summary>
public sealed class FoodSearchService : IFoodSearchService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public FoodSearchService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["CalorieNinjas:ApiKey"] 
            ?? throw new InvalidOperationException("CalorieNinjas: ApiKey не настроен в конфигурации");
        
        
        _httpClient.BaseAddress = new Uri("https://api.calorieninjas.com/v1/");
    }

    public async Task<FoodSearchResultDTO?> SearchFoodAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            throw new ArgumentException("Название продукта не может быть пустым", nameof(query));

        // Формируем запрос к CalorieNinjas API
        var request = new HttpRequestMessage(HttpMethod.Get, $"nutrition?query={Uri.EscapeDataString(query)}");
        request.Headers.Add("X-Api-Key", _apiKey);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content. ReadAsStringAsync();
            throw new HttpRequestException($"CalorieNinjas API вернул ошибку {response.StatusCode}: {errorContent}");
        }

        var result = await response.Content.ReadFromJsonAsync<CalorieNinjasResponse>();

        // Если продукт не найден
        if (result?. Items == null || result.Items.Count == 0)
            return null;

        // Берём первый результат
        var item = result.Items.First();

        return new FoodSearchResultDTO(
            FoodName: item.Name,
            ServingSize: item.ServingSizeG,
            ServingUnit: "g",
            Calories: item. Calories,
            Protein:  item. ProteinG,
            Carbs: item.CarbohydratesTotalG,
            Fats: item.FatTotalG
        );
    }

    // Модели для десериализации ответа CalorieNinjas API
    private sealed record CalorieNinjasResponse(
        [property: JsonPropertyName("items")] List<CalorieNinjasItem> Items
    );

    private sealed record CalorieNinjasItem(
        [property: JsonPropertyName("name")] string Name,
        [property:  JsonPropertyName("calories")] double Calories,
        [property:  JsonPropertyName("serving_size_g")] double ServingSizeG,
        [property: JsonPropertyName("fat_total_g")] double FatTotalG,
        [property: JsonPropertyName("protein_g")] double ProteinG,
        [property: JsonPropertyName("carbohydrates_total_g")] double CarbohydratesTotalG
    );
}