using Finbuckle.MultiTenant.Abstractions;

namespace TalentMesh.Framework.Infrastructure.Tenant.Abstractions;
public interface ITMTenantInfo : ITenantInfo
{
    string? ConnectionString { get; set; }
}
