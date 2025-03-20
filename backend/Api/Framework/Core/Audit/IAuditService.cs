using System.Diagnostics.CodeAnalysis;
namespace TalentMesh.Framework.Core.Audit;
public interface IAuditService
{
    Task<List<AuditTrail>> GetUserTrailsAsync(Guid userId);
}
