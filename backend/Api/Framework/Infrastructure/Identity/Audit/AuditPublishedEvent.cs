using System.Collections.ObjectModel;
using TalentMesh.Framework.Core.Audit;
using MediatR;

namespace TalentMesh.Framework.Infrastructure.Identity.Audit;
public class AuditPublishedEvent : INotification
{
    public AuditPublishedEvent(Collection<AuditTrail>? trails)
    {
        Trails = trails;
    }
    public Collection<AuditTrail>? Trails { get; }
}
