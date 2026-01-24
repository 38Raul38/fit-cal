using FitCal.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitCal.Persistence.Configurations;

public class UserInformationConfiguration : IEntityTypeConfiguration<UserInformation>
{
    public void Configure(EntityTypeBuilder<UserInformation> builder)
    {
        builder.Property(ui => ui.AuthUserId).IsRequired();
        builder.HasIndex(ui => ui.AuthUserId).IsUnique();
        builder.HasKey(ui => ui.UserInformationId);
        builder.Property(ui => ui.BirthDate);
        builder.Property(ui => ui.Gender).IsRequired();
        builder.Property(ui => ui.Height).IsRequired();
        builder.Property(ui => ui.Weight).IsRequired();
        builder.Property(ui => ui.WeightGoal).IsRequired();
        builder.Property(ui => ui.ActivityLevel).IsRequired();
        builder.Property(ui => ui.DailyCalories).IsRequired();
        builder.Property(ui => ui.Protein).IsRequired();
        builder.Property(ui => ui.Fats).IsRequired();
        builder.Property(ui => ui.Carbs).IsRequired();
    }
}