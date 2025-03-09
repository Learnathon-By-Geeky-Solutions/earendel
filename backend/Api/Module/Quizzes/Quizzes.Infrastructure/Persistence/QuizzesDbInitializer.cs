using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Quizzes.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Quizzes.Infrastructure.Persistence;

namespace TalentMesh.Module.Quizzes.Infrastructure.Persistence;
internal sealed class QuizzesDbInitializer(
    ILogger<QuizzesDbInitializer> logger,
    QuizzesDbContext context) : IDbInitializer
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] applied database migrations for quizzes module", context.TenantInfo!.Identifier);
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        var userId = Guid.NewGuid();
        const int TotalQuestions = 10;
        const decimal Score = 85.5m;

        if (await context.QuizAttempts.FirstOrDefaultAsync(t => t.UserId == userId, cancellationToken).ConfigureAwait(false) is null)
        {
            var quiz = Quizzes.Domain.QuizAttempt.Create(userId, Score, TotalQuestions);
            await context.QuizAttempts.AddAsync(quiz, cancellationToken);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeding default quiz data", context.TenantInfo!.Identifier);
        }
    }

}
