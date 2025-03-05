using Finbuckle.MultiTenant.Abstractions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Infrastructure.Persistence;
using TalentMesh.Framework.Infrastructure.Tenant;
using TalentMesh.Module.Notifications.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;


namespace TalentMesh.Module.Notifications.Infrastructure.Persistence;

public sealed class NotificationsDbContext : TMDbContext
{
    public NotificationsDbContext(IMultiTenantContextAccessor<TMTenantInfo> multiTenantContextAccessor, DbContextOptions<NotificationsDbContext> options, IPublisher publisher, IOptions<DatabaseOptions> settings)
        : base(multiTenantContextAccessor, options, publisher, settings)
    {
    }

    public DbSet<Notification> Notifications { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationsDbContext).Assembly);
        modelBuilder.HasDefaultSchema(SchemaNames.Notifications);
    }
}
