using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using TalentMesh.Framework.Infrastructure.SignalR;
using TalentMesh.Framework.Infrastructure.Messaging;
using TalentMesh.Framework.Infrastructure.Common;
using TalentMesh.Module.Notifications.Infrastructure.Persistence;
using TalentMesh.Module.Notifications.Domain;
using TalentMesh.Framework.Infrastructure.Identity.Users;
using Microsoft.AspNetCore.Identity;

namespace TalentMesh.Module.Notifications.Infrastructure.Messaging
{
    public class NotificationConsumer : RabbitMqConsumer<NotificationMessage>
    {
        public NotificationConsumer(
            ILogger<NotificationConsumer> logger,
            IConnectionFactory connectionFactory,
            IMessageBus messageBus,
            IServiceScopeFactory scopeFactory,
            IHubContext<NotificationHub> hubContext)
            : base(logger, connectionFactory, "notification.events", "notification.fetched", "notification.fetched", hubContext, scopeFactory)
        { }

        protected override async Task ProcessDomainMessage(NotificationMessage message, IServiceScope scope, CancellationToken stoppingToken)
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<NotificationsDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<TMUser>>(); // Replace YourUserClass with your actual IdentityUser class

            if (message.UserId == "admin")
            {
                // Fetch all admin users
                var admins = await userManager.GetUsersInRoleAsync("Admin");

                foreach (var admin in admins)
                {
                    if (!Guid.TryParse(admin.Id, out var adminUserIdGuid))
                    {
                        continue; // Skip if cannot parse
                    }

                    var notificationForAdmin = Notification.Create(
                        adminUserIdGuid,
                        message.Entity,
                        message.EntityType,
                        message.Message
                    );

                    await dbContext.Notifications.AddAsync(notificationForAdmin, stoppingToken);
                }
            }
            else
            {
                if (!Guid.TryParse(message.UserId, out var userIdGuid))
                {
                    return;
                }

                var notificationInfo = Notification.Create(
                    userIdGuid,
                    message.Entity,
                    message.EntityType,
                    message.Message
                );

                await dbContext.Notifications.AddAsync(notificationInfo, stoppingToken);
            }

            // Persist all notifications at once
            await dbContext.SaveChangesAsync(stoppingToken);
        }
        protected override Guid GetRequestedBy(NotificationMessage message) => message.RequestedBy;
    }

    public class NotificationMessage
    {
        public string? UserId { get; set; }
        public string? Entity { get; set; }
        public string? Message { get; set; }
        public string? EntityType { get; set; }
        public Guid RequestedBy { get; set; }
    }

}
