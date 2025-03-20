using TalentMesh.Module.Interviews.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Infrastructure.Persistence.Configurations;
[ExcludeFromCodeCoverage]

internal sealed class InterviewFeedbackConfiguration : IEntityTypeConfiguration<InterviewFeedback>
{
    public void Configure(EntityTypeBuilder<InterviewFeedback> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.InterviewId)
            .IsRequired(); 

        builder.Property(x => x.InterviewQuestionId)
            .IsRequired(); 

        builder.Property(x => x.Response)
            .IsRequired();  

        builder.Property(x => x.Score)
            .HasMaxLength(50) 
            .IsRequired();      
    }
}
