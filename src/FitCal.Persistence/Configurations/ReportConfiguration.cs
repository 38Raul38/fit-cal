using FitCal.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitCal.Persistence.Configurations;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.HasKey(r => r.ReportId);
        builder.Property(r => r.ReportName).IsRequired().HasMaxLength(200);
        builder.Property(r => r.ReportDate);
    }
}