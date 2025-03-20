using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Notifications.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Notifications.Infrastructure.Persistence;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Notifications.Infrastructure.Persistence
{
    [ExcludeFromCodeCoverage]

    internal sealed class NotificationsDbInitializer(
        ILogger<NotificationsDbInitializer> logger,
        NotificationsDbContext context) : IDbInitializer
    {
        public async Task MigrateAsync(CancellationToken cancellationToken)
        {
            // Apply any pending migrations
            if ((await context.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
            {
                await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] applied database migrations for notifications module", context.TenantInfo!.Identifier);
            }
        }

        public async Task SeedAsync(CancellationToken cancellationToken)
        {
            var userId = Guid.NewGuid(); // Generate a new user ID for seeding
            const string entity = "jobs"; // Entity for seeding
            const string entityType = "job"; // Entity type for seeding
            const string message = "create"; // Message for seeding

            // Check if the notification already exists for the user
            if (await context.Notifications.FirstOrDefaultAsync(t => t.UserId == userId, cancellationToken).ConfigureAwait(false) is null)
            {
                // Create a new notification for seeding
                var notification = Notification.Create(userId, entity, entityType, message);

                // Add the notification to the database
                await context.Notifications.AddAsync(notification, cancellationToken);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                // Log the seeding process
                logger.LogInformation("[{Tenant}] seeding default notification data", context.TenantInfo!.Identifier);
            }
        }
    }
}
