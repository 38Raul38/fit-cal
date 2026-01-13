using System.Text.Json.Serialization;
using FitCal.Application.Services.Classes;
using FitCal.Application.Services.Interfaces;
using FitCal.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddControllers()
//     .AddJsonOptions(o =>
//     {
//         // Чтобы enum приходили/уходили строками: "MaintainWeight", "Active"
//         o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
//     });

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<FitCalContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("FitCalDatabase")));

builder.Services.AddScoped<IFoodService, FoodService>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IUserHistoryService, UserHistoryService>();
builder.Services.AddScoped<ICalorieCalculatorService, CalorieCalculatorService>();


var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
