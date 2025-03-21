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
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] applied database migrations for experties module", context.TenantInfo!.Identifier);
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        const string Name = "Rickshaw Puller";
        const string Description = "You Drive a Tesla Around Dhaka";
        if (await context.Skills.FirstOrDefaultAsync(t => t.Name == Name, cancellationToken).ConfigureAwait(false) is null)
        {
            var skill = Experties.Domain.Skill.Create(Name, Description);
            await context.Skills.AddAsync(skill, cancellationToken);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeding default skills data", context.TenantInfo!.Identifier);
        }
    }
}
