using TalentMesh.Module.Quizzes.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TalentMesh.Module.Quizzes.Infrastructure.Persistence.Configurations;

internal sealed class QuizQuestionConfiguration : IEntityTypeConfiguration<QuizQuestion>
{
    public void Configure(EntityTypeBuilder<QuizQuestion> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.QuestionText)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.Option1)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Option2)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Option3)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Option4)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.CorrectOption)
            .IsRequired();
    }
}
