using TalentMesh.Module.Job.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TalentMesh.Module.Job.Infrastructure.Persistence.Configurations;
internal sealed class BrandConfiguration : IEntityTypeConfiguration<Job.Domain.Jobs>
{
    public void Configure(EntityTypeBuilder<Job.Domain.Jobs> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.Requirments).HasMaxLength(1000);
        builder.Property(x => x.Location).HasMaxLength(100);
        builder.Property(x => x.JobType).HasMaxLength(100);
        builder.Property(x => x.ExperienceLevel).HasMaxLength(100);
    }
}