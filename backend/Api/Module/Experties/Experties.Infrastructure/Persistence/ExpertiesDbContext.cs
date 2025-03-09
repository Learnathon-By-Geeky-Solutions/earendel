using Finbuckle.MultiTenant.Abstractions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Infrastructure.Persistence;
using TalentMesh.Framework.Infrastructure.Tenant;
using TalentMesh.Module.Experties.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;


namespace TalentMesh.Module.Experties.Infrastructure.Persistence;

public sealed class ExpertiesDbContext : TMDbContext
{
    public ExpertiesDbContext(IMultiTenantContextAccessor<TMTenantInfo> multiTenantContextAccessor, DbContextOptions<ExpertiesDbContext> options, IPublisher publisher, IOptions<DatabaseOptions> settings)
        : base(multiTenantContextAccessor, options, publisher, settings)
    {
    }

    public DbSet<Skill> Skills { get; set; } = null!;
    public DbSet<SubSkill> SubSkills { get; set; } = null!;
    public DbSet<Rubric> Rubrics { get; set; } = null!;
    public DbSet<Seniority> Seniorities { get; set; } = null!;
    public DbSet<SeniorityLevelJunction> SeniorityLevelJunctions { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExpertiesDbContext).Assembly);
        modelBuilder.HasDefaultSchema(SchemaNames.Experties);
    }
}
