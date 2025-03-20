using Finbuckle.MultiTenant.Abstractions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Infrastructure.Persistence;
using TalentMesh.Framework.Infrastructure.Tenant;
using TalentMesh.Module.Quizzes.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Infrastructure.Persistence;
[ExcludeFromCodeCoverage]

public sealed class QuizzesDbContext : TMDbContext
{
    public QuizzesDbContext(IMultiTenantContextAccessor<TMTenantInfo> multiTenantContextAccessor, DbContextOptions<QuizzesDbContext> options, IPublisher publisher, IOptions<DatabaseOptions> settings)
        : base(multiTenantContextAccessor, options, publisher, settings)
    {
    }

    public DbSet<QuizAttempt> QuizAttempts { get; set; } = null!;
    public DbSet<QuizQuestion> QuizQuestions { get; set; } = null!;
    public DbSet<QuizAttemptAnswer> QuizAttemptAnswers { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(QuizzesDbContext).Assembly);
        modelBuilder.HasDefaultSchema(SchemaNames.Quizzes);
    }
}
