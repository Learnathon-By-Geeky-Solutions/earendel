using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Candidate.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Candidate.Infrastructure.Persistence
{
    [ExcludeFromCodeCoverage]

    internal sealed class CandidateDbInitializer(
    ILogger<CandidateDbInitializer> logger,
    CandidateDbContext context) : IDbInitializer
    {
        public async Task MigrateAsync(CancellationToken cancellationToken)
        {
            if ((await context.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
            {
                await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] applied database migrations for Candidate module", context.TenantInfo!.Identifier);
            }
        }

        public async Task SeedAsync(CancellationToken cancellationToken)
        {
            // Sample data for seeding a candidate profile.
            const string sampleResume = "Sample Candidate Resume";
            // Check if a candidate profile with this sample resume already exists.
            if (await context.CandidateProfiles.FirstOrDefaultAsync(cp => cp.Resume == sampleResume, cancellationToken)
                .ConfigureAwait(false) is null)
            {
                // NOTE: Ensure that this sampleUserId corresponds to a valid seeded user in your system.
                Guid sampleUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");
                var candidateProfile = CandidateProfile.Create(
                    userId: sampleUserId,
                    resume: sampleResume,
                    skills: "C#, SQL, ASP.NET Core",
                    experience: "3 years of software development experience",
                    education: "Bachelor's in Computer Science"
                );
                await context.CandidateProfiles.AddAsync(candidateProfile, cancellationToken);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeding default candidate profile data", context.TenantInfo!.Identifier);
            }
        }
    }
}
