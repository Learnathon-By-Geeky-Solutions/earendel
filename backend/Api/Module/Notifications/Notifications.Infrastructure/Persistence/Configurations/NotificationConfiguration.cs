using TalentMesh.Module.Notifications.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Notifications.Infrastructure.Persistence.Configurations;
[ExcludeFromCodeCoverage]

internal sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        // Define the primary key for the Notification entity
        builder.HasKey(x => x.Id);

        // Configure the properties of the Notification entity
        builder.Property(x => x.UserId)
            .IsRequired();  // Ensures UserId is required

        builder.Property(x => x.Entity)
            .HasMaxLength(100)  // Limiting the length for Entity (adjust length as needed)
            .IsRequired(false); // Make it nullable

        builder.Property(x => x.EntityType)
            .HasMaxLength(50)  // Limiting the length for EntityType (adjust length as needed)
            .IsRequired(false); // Make it nullable

        builder.Property(x => x.Message)
            .HasMaxLength(1000) // Limiting the length for Message (adjust length as needed)
            .IsRequired(false); // Make it nullable
    }
}
