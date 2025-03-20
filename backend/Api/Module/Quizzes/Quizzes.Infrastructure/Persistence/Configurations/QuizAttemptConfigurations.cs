using TalentMesh.Module.Quizzes.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Infrastructure.Persistence.Configurations;
[ExcludeFromCodeCoverage]

internal sealed class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
{
    public void Configure(EntityTypeBuilder<QuizAttempt> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.TotalQuestions)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.Score)
            .IsRequired()
            .HasDefaultValue(0.0m);
    }
}
