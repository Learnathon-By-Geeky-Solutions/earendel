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
using TalentMesh.Module.Evaluator.Infrastructure.Persistence;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Evaluator.Infrastructure.Messaging
{
    [ExcludeFromCodeCoverage] 
    public class InterviewerFormConsumer : RabbitMqConsumer<InterviewerMessage>
    {
        public InterviewerFormConsumer(
            ILogger<InterviewerFormConsumer> logger,
            IConnectionFactory connectionFactory,
            IMessageBus messageBus,
            IServiceScopeFactory scopeFactory,
            IHubContext<NotificationHub> hubContext)
            : base(logger, connectionFactory, "interviewer.form.list.events", "interviewer.form.list.fetched", "interviewer.form.list.fetched", hubContext, scopeFactory)
        { }

        protected override async Task ProcessDomainMessage(InterviewerMessage message, IServiceScope scope, CancellationToken stoppingToken)
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<EvaluatorDbContext>();
            var interviewerIds = message.Interviewers.Select(i => i.Id).ToList();

            var allForms = await dbContext.InterviewerEntryForms
                .Where(j => interviewerIds.Contains(j.UserId))
                .Select(j => new FormItem
                {
                    Id = j.Id,
                    UserId = j.UserId,
                    AdditionalInfo = j.AdditionalInfo,
                    Status = j.Status,
                    CreatedAt = j.Created.UtcDateTime,
                })
                .ToListAsync(stoppingToken);

            var formsByHr = allForms.ToLookup(j => j.UserId);
            foreach (var interviewer in message.Interviewers)
            {
                interviewer.Forms = formsByHr[interviewer.Id].ToList();
            }
        }

        protected override Guid GetRequestedBy(InterviewerMessage message) => message.RequestedBy;
    }

    public class InterviewerMessage
    {
        public List<InterviewerItem> Interviewers { get; set; } = new List<InterviewerItem>();
        public Guid RequestedBy { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPendingInterviewerCount { get; set; }
        public int TotalVerifiedInterviewerCount { get; set; }
    }

    public class InterviewerItem
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
        public List<FormItem> Forms { get; set; } = new List<FormItem>();
    }

    public class FormItem
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string AdditionalInfo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
    }
}
