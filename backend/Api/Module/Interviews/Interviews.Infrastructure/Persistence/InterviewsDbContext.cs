using Finbuckle.MultiTenant.Abstractions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Infrastructure.Persistence;
using TalentMesh.Framework.Infrastructure.Tenant;
using TalentMesh.Module.Interviews.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;


namespace TalentMesh.Module.Interviews.Infrastructure.Persistence;
[ExcludeFromCodeCoverage]

public sealed class InterviewsDbContext : TMDbContext
{
    public InterviewsDbContext(IMultiTenantContextAccessor<TMTenantInfo> multiTenantContextAccessor, DbContextOptions<InterviewsDbContext> options, IPublisher publisher, IOptions<DatabaseOptions> settings)
        : base(multiTenantContextAccessor, options, publisher, settings)
    {
    }

    public DbSet<Interview> Interviews { get; set; } = null!;
    public DbSet<InterviewQuestion> InterviewQuestions { get; set; } = null!;
    public DbSet<InterviewFeedback> InterviewFeedbacks { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InterviewsDbContext).Assembly);
        modelBuilder.HasDefaultSchema(SchemaNames.Interviews);
    }
}
