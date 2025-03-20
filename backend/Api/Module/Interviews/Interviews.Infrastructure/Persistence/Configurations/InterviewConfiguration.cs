using TalentMesh.Module.Interviews.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Infrastructure.Persistence.Configurations;

[ExcludeFromCodeCoverage]

internal sealed class InterviewConfiguration : IEntityTypeConfiguration<Interview>
{
    public void Configure(EntityTypeBuilder<Interview> builder)
    {
        // Define the primary key for the Interview entity
        builder.HasKey(x => x.Id);

        // Configure the properties of the Interview entity
        builder.Property(x => x.ApplicationId)
            .IsRequired();  // Ensures ApplicationId is required

        builder.Property(x => x.InterviewerId)
            .IsRequired();  // Ensures InterviewerId is required

        builder.Property(x => x.InterviewDate)
            .IsRequired();  // Ensures InterviewDate is required

        builder.Property(x => x.Status)
            .HasMaxLength(50)  // Adjust the length for Status as needed
            .IsRequired();      // Status is required

        builder.Property(x => x.Notes)
            .HasMaxLength(1000) // Limit the length for Notes (adjust length as needed)
            .IsRequired(false);  // Notes can be nullable

        builder.Property(x => x.MeetingId)
            .HasMaxLength(100) // Limit the length for MeetingId (adjust length as needed)
            .IsRequired();  // Ensures MeetingId is required
    }
}
