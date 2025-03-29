using System;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using TalentMesh.Module.Evaluator.Infrastructure.Persistence;
using TalentMesh.Framework.Infrastructure.SignalR;
using TalentMesh.Framework.Infrastructure.Messaging;
using TalentMesh.Framework.Infrastructure.Common;

namespace TalentMesh.Module.Evaluator.Infrastructure.Messaging
{
    public class InterviewerFormConsumer : RabbitMqConsumerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IServiceScopeFactory _scopeFactory;

        public InterviewerFormConsumer(
            ILogger<InterviewerFormConsumer> logger,
            IConnectionFactory connectionFactory,
            IMessageBus messageBus,
            IServiceScopeFactory scopeFactory,
            IHubContext<NotificationHub> hubContext)
            : base(logger, connectionFactory, "interviewer.form.list.events", "interviewer.form.list.fetched", "interviewer.form.list.fetched")
        {
            _hubContext = hubContext;
            _scopeFactory = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetUpRabbitMQConnection();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (sender, ea) => await ProcessMessageAsync(ea, stoppingToken);
            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }

        private async Task ProcessMessageAsync(BasicDeliverEventArgs ea, CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Received Interviewer job list event message.");
                var messageJson = Encoding.UTF8.GetString(ea.Body.ToArray());
                var interviewerMessage = JsonHelper.Deserialize<InterviewerMessage>(messageJson);

                if (interviewerMessage == null)
                {
                    _logger.LogWarning("Failed to deserialize Interviewer job list message.");
                    _channel.BasicNack(ea.DeliveryTag, false, true);
                    return;
                }

                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<EvaluatorDbContext>();

                var interviewerIds = interviewerMessage.Interviewers.Select(interviewer => interviewer.Id).ToList();

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

                foreach (var interviewer in interviewerMessage.Interviewers)
                {
                    interviewer.Forms = formsByHr[interviewer.Id].ToList();
                }

                // Calculate total pending and verified counts across all interviewers
                int totalPendingInterviewerCount = interviewerMessage.Interviewers.Sum(i => i.Forms.Count(f => f.Status.Equals("pending", StringComparison.OrdinalIgnoreCase)));
                int totalVerifiedInterviewerCount = interviewerMessage.Interviewers.Sum(i => i.Forms.Count(f => f.Status.Equals("verified", StringComparison.OrdinalIgnoreCase)));

                var finalResponse = new
                {
                    EventType = "interviewer.form.list.updated",
                    Timestamp = DateTime.UtcNow,
                    TotalPendingInterviewerCount = totalPendingInterviewerCount,
                    TotalVerifiedInterviewerCount = totalVerifiedInterviewerCount,
                    TotalRecords = interviewerMessage.TotalRecords,
                    Interviewers = interviewerMessage.Interviewers
                };

                var finalJson = JsonHelper.Serialize(finalResponse);
                _logger.LogInformation("Final Interviewer Form List Response: {Response}", finalJson);

                await _hubContext.Clients.Group($"user:{interviewerMessage.RequestedBy}")
                    .SendAsync("ReceiveMessage", finalJson);

                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Interviewer form list message");
                _channel.BasicNack(ea.DeliveryTag, false, true);
            }
        }

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