using FitCal.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitCal.Persistence.Configurations;

public class RecipeIngridientConfiguration : IEntityTypeConfiguration<RecipeIngredient>
{
    public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
    {
        builder.HasKey(ri => ri.RecipeId);
        builder.Property(ri => ri.Quantity).IsRequired();
    }
}