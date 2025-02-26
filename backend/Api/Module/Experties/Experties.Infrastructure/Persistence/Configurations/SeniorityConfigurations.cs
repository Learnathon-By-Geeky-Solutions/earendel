using TalentMesh.Module.Experties.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TalentMesh.Module.Experties.Infrastructure.Persistence.Configurations;
internal sealed class SeniorityConfiguration : IEntityTypeConfiguration<Experties.Domain.Seniority>
{
    public void Configure(EntityTypeBuilder<Experties.Domain.Seniority> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(1000);
    }
}