using Finbuckle.MultiTenant.Abstractions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Infrastructure.Persistence;
using TalentMesh.Framework.Infrastructure.Tenant;
using TalentMesh.Module.Quizzes.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;


namespace TalentMesh.Module.Quizzes.Infrastructure.Persistence;

public sealed class QuizzesDbContext : TMDbContext
{
    public QuizzesDbContext(IMultiTenantContextAccessor<TMTenantInfo> multiTenantContextAccessor, DbContextOptions<QuizzesDbContext> options, IPublisher publisher, IOptions<DatabaseOptions> settings)
        : base(multiTenantContextAccessor, options, publisher, settings)
    {
    }

    public DbSet<QuizAttempt> QuizAttempts { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(QuizzesDbContext).Assembly);
        modelBuilder.HasDefaultSchema(SchemaNames.Quizzes);
    }
}
