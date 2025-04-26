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

            if (!Guid.TryParse(message.UserId, out var userIdGuid))
            {
                return;
            }

            var notificationInfo = Notification.Create(
                userIdGuid,
                message.Entity,
                message.EntityType,
                message.Message);

            await dbContext.Notifications.AddAsync(notificationInfo, stoppingToken);

            // Persist changes to the database (insert the notification)
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
