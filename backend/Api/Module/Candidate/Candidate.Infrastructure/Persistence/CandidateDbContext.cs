using Finbuckle.MultiTenant.Abstractions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Infrastructure.Persistence;
using TalentMesh.Framework.Infrastructure.Tenant;
using TalentMesh.Module.Candidate.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Candidate.Infrastructure.Persistence
{
    [ExcludeFromCodeCoverage]

    public sealed class CandidateDbContext : TMDbContext
    {
        public CandidateDbContext(
            IMultiTenantContextAccessor<TMTenantInfo> multiTenantContextAccessor,
            DbContextOptions<CandidateDbContext> options,
            IPublisher publisher,
            IOptions<DatabaseOptions> settings)
            : base(multiTenantContextAccessor, options, publisher, settings)
        {
        }

        public DbSet<CandidateProfile> CandidateProfiles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);
            // Apply all Candidate configurations from this assembly.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CandidateDbContext).Assembly);
            // Set the default schema for Candidate module. Adjust the schema name if needed.
            modelBuilder.HasDefaultSchema(SchemaNames.Candidate);
        }
    }
}
