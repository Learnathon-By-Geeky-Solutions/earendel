using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Job.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Job.Infrastructure.Persistence;
internal sealed class JobDbInitializer(
    ILogger<JobDbInitializer> logger,
    JobDbContext context) : IDbInitializer
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("Applied database migrations for USER module");
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        const string Name = "ABUEL";
        const string Description = "A JOBLESS PERSON";


        if (await context.Products.FirstOrDefaultAsync(t => t.Name == Name, cancellationToken).ConfigureAwait(false) is null)
        {
            var product = Job.Domain.Jobs.Create(Name, Description);
            await context.Products.AddAsync(product, cancellationToken);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("Seeding default User data");
        }
    }
}
