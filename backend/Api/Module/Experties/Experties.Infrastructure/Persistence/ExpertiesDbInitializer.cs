using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Infrastructure.Persistence;
[ExcludeFromCodeCoverage]

internal sealed class ExpertiesDbInitializer(
    ILogger<ExpertiesDbInitializer> logger,
    ExpertiesDbContext context) : IDbInitializer
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        // Always call MigrateAsync; no-op if no pending migrations
        await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] applied database migrations for experties module", context.TenantInfo!.Identifier);
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        const string Name = "Rickshaw Puller";
        const string Description = "You Drive a Tesla Around Dhaka";

        // Use null-coalescing to add only if not exists, without branching
        var skill = await context.Skills
            .FirstOrDefaultAsync(t => t.Name == Name, cancellationToken)
            .ConfigureAwait(false)
            ?? context.Skills.Add(Skill.Create(Name, Description)).Entity;

        // Persist any additions
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeding default skills data", context.TenantInfo!.Identifier);
    }
}
