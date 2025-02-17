using TalentMesh.Framework.Core.Audit;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Infrastructure.Identity.RoleClaims;
using TalentMesh.Framework.Infrastructure.Identity.Roles;
using TalentMesh.Framework.Infrastructure.Identity.Users;
using TalentMesh.Framework.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TalentMesh.Framework.Infrastructure.Identity.Persistence;

public class IdentityDbContext : IdentityDbContext<TalentMeshUser,
    TMRole,
    string,
    IdentityUserClaim<string>,
    IdentityUserRole<string>,
    IdentityUserLogin<string>,
    TMRoleClaim,
    IdentityUserToken<string>>
{
    private readonly DatabaseOptions _settings;

    public IdentityDbContext(DbContextOptions<IdentityDbContext> options, IOptions<DatabaseOptions> settings)
        : base(options)
    {
        _settings = settings.Value;
    }

    public DbSet<AuditTrail> AuditTrails { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!string.IsNullOrWhiteSpace(_settings.ConnectionString))
        {
            optionsBuilder.ConfigureDatabase(_settings.Provider, _settings.ConnectionString);
        }
    }
}
