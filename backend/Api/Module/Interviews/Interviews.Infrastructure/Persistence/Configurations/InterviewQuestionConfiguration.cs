using TalentMesh.Module.Interviews.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TalentMesh.Module.Interviews.Infrastructure.Persistence.Configurations;

internal sealed class InterviewQuestionConfiguration : IEntityTypeConfiguration<InterviewQuestion>
{
    public void Configure(EntityTypeBuilder<InterviewQuestion> builder)
    {
        // Define the primary key for the InterviewQuestion entity
        builder.HasKey(x => x.Id);

        builder.Property(x => x.RubricId)
            .IsRequired();

        builder.Property(x => x.QuestionText)
            .HasMaxLength(50)
            .IsRequired();      
    }
}
