using FitCal.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitCal.Persistence.Configurations;

public class FoodConfiguration : IEntityTypeConfiguration<Food>
{
    public void Configure(EntityTypeBuilder<Food> builder)
    {
        builder.HasKey(f => f.FoodId);

        builder.Property(f => f.Name).IsRequired().HasMaxLength(100);

        builder.Property(f => f.ServingSize).IsRequired();
        builder.Property(f => f.ServingUnit).IsRequired();

        builder.Property(f => f.Calories).IsRequired();
        builder.Property(f => f.Protein).IsRequired();
        builder.Property(f => f.Carbs).IsRequired();
        builder.Property(f => f.Fats).IsRequired();

        builder.Property(f => f.OwnerAuthUserId).IsRequired();
        builder.HasIndex(f => f.OwnerAuthUserId);
    }
}