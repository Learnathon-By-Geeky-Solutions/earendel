using TalentMesh.Module.Experties.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Infrastructure.Persistence.Configurations;
[ExcludeFromCodeCoverage]

internal sealed class SeniorityLevelJunctionConfiguration : IEntityTypeConfiguration<Experties.Domain.SeniorityLevelJunction>
{
    public void Configure(EntityTypeBuilder<Experties.Domain.SeniorityLevelJunction> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.SeniorityLevelId).IsRequired();
        builder.Property(x => x.SkillId).IsRequired();

        // Define relationships
        builder.HasOne(x => x.Seniority)
            .WithMany()
            .HasForeignKey(x => x.SeniorityLevelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Skill)
            .WithMany(s => s.SeniorityLevelJunctions)  // Ensure Skill entity has SeniorityLevelJunctions defined
            .HasForeignKey(x => x.SkillId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}