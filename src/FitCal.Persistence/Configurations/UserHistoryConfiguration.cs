using FitCal.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitCal.Persistence.Configurations;

public class UserHistoryConfiguration : IEntityTypeConfiguration<UserHistory>
{
    public void Configure(EntityTypeBuilder<UserHistory> builder)
    {
        builder.HasKey(h => h.UserInformationId);
        builder.Property(h => h.JournalDate);
    }
}