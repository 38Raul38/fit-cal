using System.Reflection;
using FitCal.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FitCal.Persistence.Context;

public class FitCalContext : DbContext
{
    public DbSet<UserHistory> UserHistories { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
    public DbSet<Food> Foods { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<UserInformation> UserInformations { get; set; }
    
    public FitCalContext(DbContextOptions<FitCalContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}