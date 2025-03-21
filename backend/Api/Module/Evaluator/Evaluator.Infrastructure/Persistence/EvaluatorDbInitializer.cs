using TalentMesh.Framework.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using TalentMesh.Module.Evaluator.Domain;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Evaluator.Infrastructure.Persistence
{
    [ExcludeFromCodeCoverage]

    internal sealed class EvaluatorDbInitializer(
        ILogger<EvaluatorDbInitializer> logger,
        EvaluatorDbContext context) : IDbInitializer
    {
        public async Task MigrateAsync(CancellationToken cancellationToken)
        {
            if ((await context.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
            {
                await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] applied database migrations for interviews module", context.TenantInfo!.Identifier);
            }
        }

        public async Task SeedAsync(CancellationToken cancellationToken)
        {
            // Sample IDs for seeding. Ensure these match existing or expected test values.
            Guid sampleJobId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            Guid sampleInterviewerId = Guid.Parse("22222222-2222-2222-2222-222222222222");

            // Seed InterviewerApplication
            const string sampleComments = "Looking forward to the interview.";
            if (await context.InterviewerApplications
                .FirstOrDefaultAsync(x => x.InterviewerId == sampleInterviewerId, cancellationToken)
                .ConfigureAwait(false) is null)
            {
                var application = InterviewerApplication.Create(sampleJobId, sampleInterviewerId, sampleComments);
                await context.InterviewerApplications.AddAsync(application, cancellationToken);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeding default interviewer application data", context.TenantInfo!.Identifier);
            }

            // Seed InterviewerAvailability
            DateTime startTime = DateTime.UtcNow.AddDays(1);
            DateTime endTime = startTime.AddHours(2);
            if (await context.InterviewerAvailabilities
                .FirstOrDefaultAsync(x => x.InterviewerId == sampleInterviewerId, cancellationToken)
                .ConfigureAwait(false) is null)
            {
                var availability = InterviewerAvailability.Create(sampleInterviewerId, startTime, endTime);
                await context.InterviewerAvailabilities.AddAsync(availability, cancellationToken);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeding default interviewer availability data", context.TenantInfo!.Identifier);
            }

            // Seed InterviewerEntryForm
            string additionalInfo = "Experienced in technical interviews and HR processes.";
            if (await context.InterviewerEntryForms
                .FirstOrDefaultAsync(x => x.UserId == sampleInterviewerId, cancellationToken)
                .ConfigureAwait(false) is null)
            {
                var entryForm = InterviewerEntryForm.Create(sampleInterviewerId, additionalInfo);
                await context.InterviewerEntryForms.AddAsync(entryForm, cancellationToken);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeding default interviewer entry form data", context.TenantInfo!.Identifier);
            }
        }
    }
}
