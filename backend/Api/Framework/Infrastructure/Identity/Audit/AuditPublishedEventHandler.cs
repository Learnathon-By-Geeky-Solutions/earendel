using TalentMesh.Framework.Core.Audit;
using TalentMesh.Framework.Infrastructure.Identity.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Identity.Audit;
[ExcludeFromCodeCoverage]

public class AuditPublishedEventHandler(ILogger<AuditPublishedEventHandler> logger, IdentityDbContext context) : INotificationHandler<AuditPublishedEvent>
{
    public async Task Handle(AuditPublishedEvent notification, CancellationToken cancellationToken)
    {
        if (context == null) return;
        logger.LogInformation("Received audit trails");

        try
        {
            await context.Set<AuditTrail>().AddRangeAsync(notification.Trails!, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while saving audit trail");
        }
    }

}
