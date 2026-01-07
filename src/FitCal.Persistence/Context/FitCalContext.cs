using System.Reflection;
using FitCal.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FitCal.Persistence.Context;

public class FitCalContext : DbContext
{
    DbSet<UserHistory> UserHistories { get; set; }
    DbSet<Recipe> Recipes { get; set; }
    DbSet<RecipeIngredient> RecipeIngredients { get; set; }
    DbSet<Food> Foods { get; set; }
    DbSet<Report> Reports { get; set; }
    DbSet<UserInformation> UserInformations { get; set; }
    
    public FitCalContext(DbContextOptions<FitCalContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}