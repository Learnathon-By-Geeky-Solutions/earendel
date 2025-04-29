using Finbuckle.MultiTenant.Abstractions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Infrastructure.Persistence;
using TalentMesh.Framework.Infrastructure.Tenant;
using TalentMesh.Module.Job.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;


namespace TalentMesh.Module.Job.Infrastructure.Persistence;
[ExcludeFromCodeCoverage]

public class JobDbContext : TMDbContext
{
    public JobDbContext(IMultiTenantContextAccessor<TMTenantInfo> multiTenantContextAccessor, DbContextOptions<JobDbContext> options, IPublisher publisher, IOptions<DatabaseOptions> settings)
        : base(multiTenantContextAccessor, options, publisher, settings)
    {
    }

    public DbSet<Jobs> Jobs { get; set; } = null!;
    public DbSet<JobApplication> JobApplications { get; set; } = null!; 
    public DbSet<JobRequiredSkill> JobRequiredSkill { get; set; } = null!;
    public DbSet<JobRequiredSubskill> JobRequiredSubskill { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(JobDbContext).Assembly);
        modelBuilder.HasDefaultSchema(SchemaNames.Job);
    }
}
