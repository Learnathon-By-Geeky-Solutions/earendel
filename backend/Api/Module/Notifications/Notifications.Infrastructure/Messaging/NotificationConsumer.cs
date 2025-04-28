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
using TalentMesh.Module.Job.Infrastructure.Persistence;
using TalentMesh.Framework.Core.Jobs;
using System.Collections.ObjectModel;
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
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<TMUser>>(); // Replace YourUserClass with your actual IdentityUser class
            var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<NotificationHub>>(); // resolve again inside scope


            if (message.EntityType == "InterviewScheduled.HR" && !string.IsNullOrEmpty(message.Entity))
            {
                var jobId = Guid.Parse(message.Entity);

                var job = await jobDbContext.Jobs.FindAsync(new object[] { jobId }, cancellationToken: stoppingToken);
                var user = await userManager.FindByIdAsync(job!.PostedById.ToString());

                // Create the notification
                var notification = Notification.Create(
                    job!.PostedById,
                    jobId.ToString(),
                    message.EntityType,
                    message.Message
                );

                await dbContext.Notifications.AddAsync(notification, stoppingToken);
                await hubContext.Clients.Group($"user:{job.PostedById}").SendAsync("SystemAlert", message.Message, stoppingToken);

                var mailRequest = new MailRequest(
                                    new Collection<string> { user!.Email! },
                                    "Interview Scheduled",
                                    message.Message! 
                                );

                _jobService.Enqueue("email", () => _mailService.SendEmail(mailRequest));

            }

            else if (message.EntityType == "InterviewScheduled.Candidate" && Guid.TryParse(message.UserId, out var candidateId) && Guid.TryParse(message.Entity, out var candidateJobId))
            {
                // 1) Create & save candidate notification
                var candidateNotification = Notification.Create(
                    candidateId,
                    candidateJobId.ToString(),
                    message.EntityType,
                    message.Message
                );
                await dbContext.Notifications.AddAsync(candidateNotification, stoppingToken);

                // 2) SignalR alert
                await hubContext.Clients
                                .Group($"user:{candidateId}")
                                .SendAsync("SystemAlert", message.Message, stoppingToken);

                // 3) Email to candidate
                var candidateUser = await userManager.FindByIdAsync(candidateId.ToString());
                if (candidateUser?.Email != null)
                {
                    var mailRequest = new MailRequest(
                        new Collection<string> { candidateUser.Email },
                        "Interview Scheduled",
                        message.Message!
                    );
                    _jobService.Enqueue("email", () => _mailService.SendEmail(mailRequest));
                }
            }

            else if (message.EntityType == "Interview.Interviewer" && Guid.TryParse(message.UserId, out var userId) && Guid.TryParse(message.Entity, out var interviewerJobId))
            {
                // 1) Create & save candidate notification
                var candidateNotification = Notification.Create(
                    userId,
                    interviewerJobId.ToString(),
                    message.EntityType,
                    message.Message
                );
                await dbContext.Notifications.AddAsync(candidateNotification, stoppingToken);

                // 2) SignalR alert
                await hubContext.Clients
                                .Group($"user:{userId}")
                                .SendAsync("SystemAlert", message.Message, stoppingToken);

                // 3) Email to candidate
                var candidateUser = await userManager.FindByIdAsync(userId.ToString());
                if (candidateUser?.Email != null)
                {
                    var mailRequest = new MailRequest(
                        new Collection<string> { candidateUser.Email },
                        "Interview Request",
                        message.Message!
                    );
                    _jobService.Enqueue("email", () => _mailService.SendEmail(mailRequest));
                }
            }

            else if (message.EntityType == "JobApplication" && !string.IsNullOrEmpty(message.Entity))
            {
                // Fetch the Job
                var jobId = Guid.Parse(message.Entity);
                var job = await jobDbContext.Jobs.FindAsync(new object[] { jobId }, cancellationToken: stoppingToken);

                var notification = Notification.Create(
                            job!.PostedById,
                            message.Entity,
                            message.EntityType,
                            message.Message
                        );

                await dbContext.Notifications.AddAsync(notification, stoppingToken);
                await dbContext.SaveChangesAsync(stoppingToken);
                await hubContext.Clients.Group($"user:{job.PostedById}").SendAsync("SystemAlert", message.Message, stoppingToken);

            }

            else if (message.UserId == "admin")
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
                    await hubContext.Clients.Group("admin").SendAsync("SystemAlert", message.Message, stoppingToken);

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
                await hubContext.Clients.Group($"user:{userIdGuid}").SendAsync("SystemAlert", message.Message, stoppingToken);
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
