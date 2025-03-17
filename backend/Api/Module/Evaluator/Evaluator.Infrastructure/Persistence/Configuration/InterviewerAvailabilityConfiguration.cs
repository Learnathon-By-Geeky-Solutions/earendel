
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentMesh.Module.Evaluator.Domain;

namespace TalentMesh.Module.Evaluator.Infrastructure.Persistence.Configurations
{
    internal sealed class InterviewerAvailabilityConfiguration : IEntityTypeConfiguration<InterviewerAvailability>
    {
        public void Configure(EntityTypeBuilder<InterviewerAvailability> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.InterviewerId)
                   .IsRequired();
            builder.Property(x => x.StartTime)
                   .IsRequired();
            builder.Property(x => x.EndTime)
                   .IsRequired();
            builder.Property(x => x.IsAvailable)
                   .IsRequired();
        }
    }
}
