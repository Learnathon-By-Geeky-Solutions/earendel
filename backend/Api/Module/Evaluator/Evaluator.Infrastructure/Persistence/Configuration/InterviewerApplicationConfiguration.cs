
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentMesh.Module.Evaluator.Domain;

namespace TalentMesh.Module.Evaluator.Infrastructure.Persistence.Configurations
{
    internal sealed class InterviewerApplicationConfiguration : IEntityTypeConfiguration<InterviewerApplication>
    {
        public void Configure(EntityTypeBuilder<InterviewerApplication> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.JobId)
                   .IsRequired();
            builder.Property(x => x.InterviewerId)
                   .IsRequired();
            builder.Property(x => x.AppliedDate)
                   .IsRequired();
            builder.Property(x => x.Status)
                   .IsRequired()
                   .HasMaxLength(50);
            builder.Property(x => x.Comments)
                   .HasMaxLength(1000);
        }
    }
}
