using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Framework.Core.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.Emit;

namespace TalentMesh.Framework.Infrastructure.Persistence;
public class ApplicationDbContext : DbContext
{
    private readonly DatabaseOptions _settings;

    public ApplicationDbContext(DbContextOptions options, IOptions<DatabaseOptions> settings)
        : base(options)
    {
        _settings = settings.Value;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply soft delete filter if you want to keep this feature
        modelBuilder.AppendGlobalQueryFilter<ISoftDeletable>(s => s.Deleted == null);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Enable sensitive data logging for development
        optionsBuilder.EnableSensitiveDataLogging();

        // If you need to configure the database connection here instead of startup
        // optionsBuilder.UseSqlServer("your_connection_string"); // or UseNpgsql, etc.
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}