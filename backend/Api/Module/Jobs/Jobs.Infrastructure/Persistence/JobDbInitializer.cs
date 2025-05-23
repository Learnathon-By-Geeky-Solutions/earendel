﻿using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Job.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Job.Infrastructure.Persistence;
[ExcludeFromCodeCoverage]

internal sealed class JobDbInitializer(
    ILogger<JobDbInitializer> logger,
    JobDbContext context) : IDbInitializer
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] applied database migrations for Job module", context.TenantInfo!.Identifier);
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        const string Name = "Rickshaw Puller";
        const string Description = "You Drive a Tesla Around Dhaka";
        const string Requirments = "Must be able to drive a Tesla";
        const string Location = "Dhaka";
        const string JobType = "Driver";
        const string ExperienceLevel = "Entry Level";
        Guid PostedBy = Guid.NewGuid();
        const string Salary = "10000";

        if (await context.Jobs.FirstOrDefaultAsync(t => t.Name == Name, cancellationToken).ConfigureAwait(false) is null)
        {
            var jobInfo = new JobInfo
            {
                Name = Name,
                Description = Description,
                Requirments = Requirments,
                Location = Location,
                JobType = JobType,
                ExperienceLevel = ExperienceLevel,
                Salary = Salary,
                PostedById = PostedBy
            };
            var product = Job.Domain.Jobs.Create(jobInfo);
            await context.Jobs.AddAsync(product, cancellationToken);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeding default catalog data", context.TenantInfo!.Identifier);
        }
    }
}
