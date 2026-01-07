using FitCal.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitCal.Persistence.Configurations;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.HasKey(r => r.RecipeId);
        builder.Property(r => r.RecipeName).IsRequired().HasMaxLength(200);
        builder.Property(r => r.ServingSize).IsRequired();
        builder.Property(r => r.ServingUnit).IsRequired().HasMaxLength(50);
        
        //Many-to-One: Recipe to UserInformation
    }
}