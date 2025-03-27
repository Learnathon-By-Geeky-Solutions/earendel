using System;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Threading;
using System.Threading.Tasks;
using TalentMesh.Module.Job.Infrastructure.Persistence;
using TalentMesh.Framework.Infrastructure.Messaging;
using TalentMesh.Framework.Infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace TalentMesh.Module.Job.Infrastructure.Messaging
{
    public class InterviewApplicationConsumer : RabbitMqConsumerBase
    {
        private readonly IMessageBus _messageBus;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IHubContext<NotificationHub> _hubContext;

        public InterviewApplicationConsumer(
            ILogger<InterviewApplicationConsumer> logger,
            IConnectionFactory connectionFactory,
            IServiceScopeFactory scopeFactory,
            IMessageBus messageBus,
            IHubContext<NotificationHub> hubContext)
            : base(logger, connectionFactory, "interview.application.events", "interview.application.getCandidate", "interview.application.getCandidate")
        {
            _connectionFactory = connectionFactory;
            _scopeFactory = scopeFactory;
            _messageBus = messageBus;
            _hubContext = hubContext;
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
            _logger.LogInformation("Received Interview Application message");

            var messageJson = Encoding.UTF8.GetString(ea.Body.ToArray());
            var interviewMessage = DeserializeMessage(messageJson);
            if (interviewMessage == null)
            {
                _logger.LogWarning("Message deserialization resulted in null.");
                return;
            }

            await ProcessInterviewsAsync(interviewMessage, stoppingToken);

            // Publish the updated message via the message bus.
            await PublishMessageAsync(interviewMessage);

            // Also send the final message to connected SignalR clients.
            var finalJson = SerializeMessage(interviewMessage);
            await _hubContext.Clients.Group($"user:{interviewMessage.UserId}").SendAsync("ReceiveMessage", finalJson);

            _channel.BasicAck(ea.DeliveryTag, multiple: false);
        }

        private InterviewMessage? DeserializeMessage(string messageJson)
        {
            try
            {
                return JsonSerializer.Deserialize<InterviewMessage>(messageJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deserializing message: {ex.Message}");
                return null;
            }
        }

        private async Task ProcessInterviewsAsync(InterviewMessage interviewMessage, CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<JobDbContext>();

            if (dbContext == null)
            {
                _logger.LogWarning("Database context is null!");
                return;
            }

            foreach (var interview in interviewMessage.Interviews)
            {
                var jobApplication = await dbContext.JobApplications
                    .FirstOrDefaultAsync(j => j.Id == interview.ApplicationId, stoppingToken);

                if (jobApplication != null)
                {
                    interview.CandidateId = jobApplication.CandidateId;
                    interview.JobId = jobApplication.JobId;

                    var job = await dbContext.Jobs
                        .FirstOrDefaultAsync(j => j.Id == interview.JobId, stoppingToken);
                    if (job != null)
                    {
                        interview.JobTitle = job.Name;
                        interview.JobDescription = job.Description;
                    }
                }
            }

            _logger.LogInformation("Finished processing interviews.");
        }

        private string SerializeMessage(InterviewMessage interviewMessage)
        {
            return JsonSerializer.Serialize(interviewMessage);
        }

        private async Task PublishMessageAsync(InterviewMessage interviewMessage)
        {
            try
            {
                await _messageBus.PublishAsync(interviewMessage, "interview.events.user", "interview.getCandidate");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error publishing message: {ex.Message}");
            }
        }
    }

    public class InterviewMessage
    {
        public Guid InterviewerId { get; set; }
        public Guid UserId { get; set; }
        public List<InterviewItem> Interviews { get; set; } = new List<InterviewItem>();
    }

    public class InterviewItem
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime InterviewDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public string MeetingId { get; set; } = string.Empty;
        public Guid? CandidateId { get; set; }
        public Guid? JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string JobDescription { get; set; } = string.Empty;
    }
}
