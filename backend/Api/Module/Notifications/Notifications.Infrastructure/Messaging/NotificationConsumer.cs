using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using TalentMesh.Framework.Infrastructure.Common;
using TalentMesh.Framework.Infrastructure.Identity.Users;
using TalentMesh.Framework.Infrastructure.Messaging;
using TalentMesh.Framework.Infrastructure.SignalR;
using TalentMesh.Module.Job.Infrastructure.Persistence;
using TalentMesh.Module.Notifications.Domain;
using TalentMesh.Module.Notifications.Infrastructure.Persistence;
using TalentMesh.Framework.Core.Jobs;
using TalentMesh.Framework.Core.Mail;

namespace TalentMesh.Module.Notifications.Infrastructure.Messaging
{
    public class NotificationConsumer : RabbitMqConsumer<NotificationMessage>
    {
        private readonly IJobService _jobService;
        private readonly IMailService _mailService;

        public NotificationConsumer(
            ILogger<NotificationConsumer> logger,
            IConnectionFactory connectionFactory,
            IMessageBus messageBus,
            IServiceScopeFactory scopeFactory,
            IJobService jobService,
            IMailService mailService,
            IHubContext<NotificationHub> hubContext)
            : base(logger, connectionFactory, "notification.events", "notification.fetched", "notification.fetched", hubContext, scopeFactory)
        {
            _jobService = jobService;
            _mailService = mailService;
        }

        protected override async Task ProcessDomainMessage(NotificationMessage message, IServiceScope scope, CancellationToken stoppingToken)
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<NotificationsDbContext>();
            var jobDbContext = scope.ServiceProvider.GetRequiredService<JobDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<TMUser>>();
            var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<NotificationHub>>();

            switch (message.EntityType)
            {
                case "InterviewScheduled.HR":
                    await HandleHrInterviewScheduled(message, jobDbContext, userManager, dbContext, stoppingToken);
                    break;

                case "InterviewScheduled.Candidate":
                    await HandleCandidateInterviewScheduled(message, userManager, dbContext, hubContext, stoppingToken);
                    break;

                case "Interview.Interviewer":
                    await HandleInterviewerNotification(message, userManager, dbContext, hubContext, stoppingToken);
                    break;

                case "JobApplication":
                    await HandleJobApplication(message, jobDbContext, dbContext, hubContext, stoppingToken);
                    break;

                default:
                    if (message.UserId == "admin")
                        await HandleAdminNotification(message, userManager, dbContext, hubContext, stoppingToken);
                    else
                        await HandleGenericUserNotification(message, dbContext, hubContext, stoppingToken);
                    break;
            }

            await dbContext.SaveChangesAsync(stoppingToken);
        }

        protected override Guid GetRequestedBy(NotificationMessage message) => message.RequestedBy;

        private async Task HandleHrInterviewScheduled(NotificationMessage message, JobDbContext jobDbContext, UserManager<TMUser> userManager, NotificationsDbContext dbContext, CancellationToken stoppingToken)
        {
            if (string.IsNullOrEmpty(message.Entity)) return;

            var jobId = Guid.Parse(message.Entity);
            var job = await jobDbContext.Jobs.FindAsync(new object[] { jobId }, cancellationToken: stoppingToken);
            var user = await userManager.FindByIdAsync(job!.PostedById.ToString());

            var notification = Notification.Create(job.PostedById, message.Entity, message.EntityType, message.Message);
            await dbContext.Notifications.AddAsync(notification, stoppingToken);

            if (!string.IsNullOrEmpty(user?.Email))
            {
                var mailRequest = new MailRequest(
                    new Collection<string> { user.Email },
                    "Interview Scheduled",
                    message.Message!
                );
                _jobService.Enqueue("email", () => _mailService.SendEmail(mailRequest));
            }
        }

        private async Task HandleCandidateInterviewScheduled(NotificationMessage message, UserManager<TMUser> userManager, NotificationsDbContext dbContext, IHubContext<NotificationHub> hubContext, CancellationToken stoppingToken)
        {
            if (!Guid.TryParse(message.UserId, out var candidateId) || !Guid.TryParse(message.Entity, out var jobId))
                return;

            var notification = Notification.Create(candidateId, jobId.ToString(), message.EntityType, message.Message);
            await dbContext.Notifications.AddAsync(notification, stoppingToken);

            await hubContext.Clients.Group($"user:{candidateId}")
                .SendAsync(NotificationConstants.SystemAlert, message.Message, stoppingToken);

            var candidateUser = await userManager.FindByIdAsync(candidateId.ToString());
            if (!string.IsNullOrEmpty(candidateUser?.Email))
            {
                var mailRequest = new MailRequest(
                    new Collection<string> { candidateUser.Email },
                    "Interview Scheduled",
                    message.Message!
                );
                _jobService.Enqueue("email", () => _mailService.SendEmail(mailRequest));
            }
        }

        private async Task HandleInterviewerNotification(NotificationMessage message, UserManager<TMUser> userManager, NotificationsDbContext dbContext, IHubContext<NotificationHub> hubContext, CancellationToken stoppingToken)
        {
            if (!Guid.TryParse(message.UserId, out var userId) || !Guid.TryParse(message.Entity, out var jobId))
                return;

            var notification = Notification.Create(userId, jobId.ToString(), message.EntityType, message.Message);
            await dbContext.Notifications.AddAsync(notification, stoppingToken);

            await hubContext.Clients.Group($"user:{userId}")
                .SendAsync(NotificationConstants.SystemAlert, message.Message, stoppingToken);

            var user = await userManager.FindByIdAsync(userId.ToString());
            if (!string.IsNullOrEmpty(user?.Email))
            {
                var mailRequest = new MailRequest(
                    new Collection<string> { user.Email },
                    "Interview Request",
                    message.Message!
                );
                _jobService.Enqueue("email", () => _mailService.SendEmail(mailRequest));
            }
        }

        private async Task HandleJobApplication(NotificationMessage message, JobDbContext jobDbContext, NotificationsDbContext dbContext, IHubContext<NotificationHub> hubContext, CancellationToken stoppingToken)
        {
            if (string.IsNullOrEmpty(message.Entity)) return;

            var jobId = Guid.Parse(message.Entity);
            var job = await jobDbContext.Jobs.FindAsync(new object[] { jobId }, cancellationToken: stoppingToken);

            var notification = Notification.Create(job!.PostedById, message.Entity, message.EntityType, message.Message);
            await dbContext.Notifications.AddAsync(notification, stoppingToken);

            await hubContext.Clients.Group($"user:{job.PostedById}")
                .SendAsync(NotificationConstants.SystemAlert, message.Message, stoppingToken);
        }

        private async Task HandleAdminNotification(NotificationMessage message, UserManager<TMUser> userManager, NotificationsDbContext dbContext, IHubContext<NotificationHub> hubContext, CancellationToken stoppingToken)
        {
            var admins = await userManager.GetUsersInRoleAsync("Admin");

            foreach (var admin in admins)
            {
                if (!Guid.TryParse(admin.Id, out var adminId)) continue;

                var notification = Notification.Create(adminId, message.Entity, message.EntityType, message.Message);
                await dbContext.Notifications.AddAsync(notification, stoppingToken);

                await hubContext.Clients.Group("admin")
                    .SendAsync(NotificationConstants.SystemAlert, message.Message, stoppingToken);
            }
        }

        private async Task HandleGenericUserNotification(NotificationMessage message, NotificationsDbContext dbContext, IHubContext<NotificationHub> hubContext, CancellationToken stoppingToken)
        {
            if (!Guid.TryParse(message.UserId, out var userId)) return;

            var notification = Notification.Create(userId, message.Entity, message.EntityType, message.Message);
            await dbContext.Notifications.AddAsync(notification, stoppingToken);

            await hubContext.Clients.Group($"user:{userId}")
                .SendAsync(NotificationConstants.SystemAlert, message.Message, stoppingToken);
        }
    }

    public static class NotificationConstants
    {
        public const string SystemAlert = "SystemAlert";
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
