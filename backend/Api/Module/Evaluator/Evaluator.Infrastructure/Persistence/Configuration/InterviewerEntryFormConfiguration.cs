
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentMesh.Module.Evaluator.Domain;

namespace TalentMesh.Module.Evaluator.Infrastructure.Persistence.Configurations
{
    internal sealed class InterviewerEntryFormConfiguration : IEntityTypeConfiguration<InterviewerEntryForm>
    {
        public void Configure(EntityTypeBuilder<InterviewerEntryForm> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId)
                   .IsRequired();
            builder.Property(x => x.Status)
                   .IsRequired()
                   .HasMaxLength(50);
            builder.Property(x => x.AdditionalInfo)
                   .HasMaxLength(2000);
        }
    }
}
