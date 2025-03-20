using TalentMesh.Module.Quizzes.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Infrastructure.Persistence.Configurations;
[ExcludeFromCodeCoverage]

internal sealed class QuizAttemptAnswerConfiguration : IEntityTypeConfiguration<QuizAttemptAnswer>
{
    public void Configure(EntityTypeBuilder<QuizAttemptAnswer> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AttemptId)
            .IsRequired();

        builder.Property(x => x.QuestionId)
            .IsRequired();

        builder.Property(x => x.SelectedOption)
            .IsRequired();

        builder.Property(x => x.IsCorrect)
            .IsRequired();

    }
}
