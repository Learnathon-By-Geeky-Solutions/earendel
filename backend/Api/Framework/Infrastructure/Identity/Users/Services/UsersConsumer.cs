using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TalentMesh.Shared.Authorization;
using TalentMesh.Framework.Infrastructure.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace TalentMesh.Framework.Infrastructure.Identity.Users.Services
{
    public class UsersConsumer : BackgroundService
    {
        private readonly ILogger<UsersConsumer> _logger;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IServiceScopeFactory _scopeFactory; // Scope factory to get UserManager
        private IConnection? _connection;
        private IModel? _channel;
        private const string ExchangeName = "interview.events.user";
        private const string QueueName = "interview.getCandidate";

        public UsersConsumer(
            ILogger<UsersConsumer> logger,
            IConnectionFactory connectionFactory,
            IHubContext<NotificationHub> hubContext,
            IServiceScopeFactory scopeFactory) // Inject scope factory
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare the exchange and queue, and bind them together
            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, durable: true);
            _channel.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(QueueName, ExchangeName, "interview.getCandidate");

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (sender, ea) => await ProcessMessageAsync(ea, stoppingToken);

            _channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        private async Task ProcessMessageAsync(BasicDeliverEventArgs ea, CancellationToken stoppingToken)
        {
            _logger.LogInformation("Received interview message(user)");

            var body = ea.Body.ToArray();
            var messageJson = Encoding.UTF8.GetString(body);

            var interviewMessage = DeserializeMessage(messageJson);
            if (interviewMessage == null)
            {
                _logger.LogWarning("Message deserialization failed.");
                return;
            }

            using var scope = _scopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<TMUser>>();

            foreach (var interview in interviewMessage.Interviews)
            {
                if (interview.CandidateId.HasValue)
                {
                    var candidate = await userManager.FindByIdAsync(interview.CandidateId.Value.ToString());
                    if (candidate != null)
                    {
                        interview.CandidateName = candidate.UserName; // Assuming UserName stores the candidate's full name
                        interview.CandidateEmail = candidate.Email;
                    }
                }
            }

            // Log updated message
            _logger.LogInformation("Updated Interview Message: {Message}", JsonSerializer.Serialize(interviewMessage));
            await _hubContext.Clients.Group($"user:{interviewMessage.UserId}").SendAsync("ReceiveMessage", interviewMessage);

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
        public string? CandidateName { get; set; } // New field
        public string? CandidateEmail { get; set; } // New field
        public Guid? JobId { get; set; }
    }
}