using Finbuckle.MultiTenant.EntityFrameworkCore.Stores.EFCoreStore;
using Microsoft.EntityFrameworkCore;

namespace TalentMesh.Framework.Infrastructure.Tenant.Persistence;
public class TenantDbContext : EFCoreStoreDbContext<TMTenantInfo>
{
    public const string Schema = "tenant";
    public TenantDbContext(DbContextOptions<TenantDbContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TMTenantInfo>().ToTable("Tenants", Schema);
    }
}
