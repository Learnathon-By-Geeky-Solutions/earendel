using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Framework.Infrastructure.Messaging;
using TalentMesh.Module.Job.Infrastructure.Persistence;

namespace TalentMesh.Module.Job.Infrastructure.Messaging
{
    public class InterviewApplicationConsumer : BackgroundService
    {
        private readonly ILogger<InterviewApplicationConsumer> _logger;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IServiceScopeFactory _scopeFactory; // Use scope factory
        private readonly IMessageBus _messageBus;
        private IConnection? _connection;
        private IModel? _channel;
        private const string ExchangeName = "interview.application.events";
        private const string QueueName = "interview.application.getCandidate";

        public InterviewApplicationConsumer(ILogger<InterviewApplicationConsumer> logger, IConnectionFactory connectionFactory, IServiceScopeFactory scopeFactory, IMessageBus messageBus)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
            _scopeFactory = scopeFactory;
            _messageBus = messageBus;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetUpRabbitMQConnection();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (sender, ea) => await ProcessMessageAsync(ea, stoppingToken);

            _channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        private void SetUpRabbitMQConnection()
        {
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare the exchange and queue, and bind them together
            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, durable: true);
            _channel.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(QueueName, ExchangeName, "interview.application.getCandidate");
        }

        private async Task ProcessMessageAsync(BasicDeliverEventArgs ea, CancellationToken stoppingToken)
        {
            _logger.LogInformation("Received JobApplication message");

            var messageJson = Encoding.UTF8.GetString(ea.Body.ToArray());
            var interviewMessage = DeserializeMessage(messageJson);

            if (interviewMessage == null)
            {
                _logger.LogWarning("Message deserialization resulted in null.");
                return;
            }

            await ProcessInterviewsAsync(interviewMessage, stoppingToken);

            var updatedMessageJson = SerializeMessage(interviewMessage);
            await PublishMessageAsync(interviewMessage);

            // Acknowledge message processing
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
            var _dbContext = scope.ServiceProvider.GetRequiredService<JobDbContext>();

            if (_dbContext == null)
            {
                _logger.LogWarning("Database context (_dbContext) is null!");
                return;
            }

            foreach (var interview in interviewMessage.Interviews)
            {
                var jobApplication = await _dbContext.JobApplications
                    .FirstOrDefaultAsync(j => j.Id == interview.ApplicationId, stoppingToken);

                if (jobApplication != null)
                {
                    interview.CandidateId = jobApplication.CandidateId;
                    interview.JobId = jobApplication.JobId;

                    // Fetch job details (JobTitle and Description) using JobId
                    var job = await _dbContext.Jobs
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

        private async Task PublishMessageAsync(dynamic updatedMessageJson)
        {
            try
            {
                await _messageBus.PublishAsync(updatedMessageJson, "interview.events.user", "interview.getCandidate");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error publishing message: {ex.Message}");
            }
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
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
