using Finbuckle.MultiTenant.Abstractions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Infrastructure.Persistence;
using TalentMesh.Framework.Infrastructure.Tenant;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Reflection.Emit;
using TalentMesh.Module.Evaluator.Domain;

namespace TalentMesh.Module.Evaluator.Infrastructure.Persistence
{
    public sealed class EvaluatorDbContext : TMDbContext
    {
        public EvaluatorDbContext(
            IMultiTenantContextAccessor<TMTenantInfo> multiTenantContextAccessor,
            DbContextOptions<EvaluatorDbContext> options,
            IPublisher publisher,
            IOptions<DatabaseOptions> settings)
            : base(multiTenantContextAccessor, options, publisher, settings)
        {
        }

        public DbSet<InterviewerApplication> InterviewerApplications { get; set; } = null!;
        public DbSet<InterviewerAvailability> InterviewerAvailabilities { get; set; } = null!;
        public DbSet<InterviewerEntryForm> InterviewerEntryForms { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EvaluatorDbContext).Assembly);
            modelBuilder.HasDefaultSchema(SchemaNames.Evaluator);
        }
    }
}
