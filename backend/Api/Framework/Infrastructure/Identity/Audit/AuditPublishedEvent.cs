using System.Collections.ObjectModel;
using TalentMesh.Framework.Core.Audit;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Identity.Audit;
[ExcludeFromCodeCoverage]

public class AuditPublishedEvent : INotification
{
    public AuditPublishedEvent(Collection<AuditTrail>? trails)
    {
        Trails = trails;
    }
    public Collection<AuditTrail>? Trails { get; }
}
