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
using TalentMesh.Module.Job.Infrastructure.Persistence;

namespace TalentMesh.Module.Job.Infrastructure.Messaging
{
    public class JobPaymentUpdatedConsumer : RabbitMqConsumer<JobMessage>
    {
        public JobPaymentUpdatedConsumer(
            ILogger<JobPaymentUpdatedConsumer> logger,
            IConnectionFactory connectionFactory,
            IMessageBus messageBus,
            IServiceScopeFactory scopeFactory,
            IHubContext<NotificationHub> hubContext)
            : base(logger, connectionFactory, "job.payment.update.events", "job.payment.update.fetched", "job.payment.update.fetched", hubContext, scopeFactory)
        { }

        protected override async Task ProcessDomainMessage(JobMessage message, IServiceScope scope, CancellationToken stoppingToken)
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<JobDbContext>();

            var job = await dbContext.Jobs
            .FirstOrDefaultAsync(j => j.Id == Guid.Parse(message.JobId!), stoppingToken);

            if (job != null)
            {
                job.PaymentStatus = message.Status!; // Or use an enum if that's how you're handling status
                await dbContext.SaveChangesAsync(stoppingToken);
            }

        }

        protected override Guid GetRequestedBy(JobMessage message) => message.RequestedBy;
    }

    public class JobMessage
    {
        public string? JobId { get; set; }
        public string? Status { get; set; }
        public Guid RequestedBy { get; set; }
    }

}
