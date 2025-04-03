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
    public class HrJobListConsumer : RabbitMqConsumer<HrMessage>
    {
        public HrJobListConsumer(
            ILogger<HrJobListConsumer> logger,
            IConnectionFactory connectionFactory,
            IMessageBus messageBus,
            IServiceScopeFactory scopeFactory,
            IHubContext<NotificationHub> hubContext)
            : base(logger, connectionFactory, "hr.job.list.events", "hr.job.list.fetched", "hr.job.list.fetched", hubContext, scopeFactory)
        { }

        protected override async Task ProcessDomainMessage(HrMessage message, IServiceScope scope, CancellationToken stoppingToken)
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<JobDbContext>();
            var hrIds = message.HRs.Select(hr => hr.Id).ToList();

            var allJobs = await dbContext.Jobs
                .Where(j => hrIds.Contains(j.CreatedBy))
                .Select(j => new JobItem
                {
                    Id = j.Id,
                    Title = j.Name,
                    Description = j.Description,
                    CreatedAt = j.Created.UtcDateTime,
                    PostedBy = j.CreatedBy
                })
                .ToListAsync(stoppingToken);

            var jobsByHr = allJobs.ToLookup(j => j.PostedBy);
            foreach (var hr in message.HRs)
            {
                hr.Jobs = jobsByHr[hr.Id].ToList();
                hr.JobCount = hr.Jobs.Count;
            }

            if (!string.IsNullOrEmpty(message.SortBy) &&
                message.SortBy.Equals("jobcount", StringComparison.OrdinalIgnoreCase))
            {
                bool isAscending = string.Equals(message.SortDirection, "asc", StringComparison.OrdinalIgnoreCase);
                message.HRs = isAscending
                    ? message.HRs.OrderBy(hr => hr.JobCount).ToList()
                    : message.HRs.OrderByDescending(hr => hr.JobCount).ToList();
            }
        }

        protected override Guid GetRequestedBy(HrMessage message) => message.RequestedBy;
    }

    public class HrMessage
    {
        public List<HrItem> HRs { get; set; } = new List<HrItem>();
        public Guid RequestedBy { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
        public int TotalRecords { get; set; }
    }

    public class HrItem
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
        public int JobCount { get; set; }
        public List<JobItem> Jobs { get; set; } = new List<JobItem>();
    }

    public class JobItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
        public Guid PostedBy { get; set; }
    }
}
