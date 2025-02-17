
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Infrastructure.Persistence;
using TalentMesh.Module.Job.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace TalentMesh.Module.Job.Infrastructure.Persistence;

public sealed class JobDbContext : DbContext
{
    // Constructor now only requires the EF Core DbContextOptions.
    public JobDbContext(DbContextOptions<JobDbContext> options)
        : base(options)
    {
    }

    // DbSet properties for your domain entities.
    public DbSet<Job.Domain.Jobs> Products { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        // Apply all configurations from the current assembly.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(JobDbContext).Assembly);

        // Set the default schema. You can either keep the constant or use a literal.
        modelBuilder.HasDefaultSchema("Jobs");
        // GG
    }
}
