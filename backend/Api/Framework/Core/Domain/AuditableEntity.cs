using TalentMesh.Framework.Core.Domain.Contracts;
using System.Diagnostics.CodeAnalysis;
namespace TalentMesh.Framework.Core.Domain;
[ExcludeFromCodeCoverage]
public class AuditableEntity<TId> : BaseEntity<TId>, IAuditable, ISoftDeletable
{
    public DateTimeOffset Created { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset LastModified { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTimeOffset? Deleted { get; set; }
    public Guid? DeletedBy { get; set; }
    

}
[ExcludeFromCodeCoverage]

public abstract class AuditableEntity : AuditableEntity<Guid>
{
    protected AuditableEntity() => Id = Guid.NewGuid();
}
