using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using TalentMesh.Framework.Infrastructure.Messaging;
using TalentMesh.Framework.Infrastructure.SignalR;
using TalentMesh.Shared.Authorization;

namespace TalentMesh.Framework.Infrastructure.Identity.Users.Services
{
    public class UsersConsumer : RabbitMqConsumerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IServiceScopeFactory _scopeFactory;

        public UsersConsumer(
            ILogger<UsersConsumer> logger,
            IConnectionFactory connectionFactory,
            IHubContext<NotificationHub> hubContext,
            IServiceScopeFactory scopeFactory)
            : base(logger, connectionFactory, "interview.events.user", "interview.getCandidate", "interview.getCandidate")
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
            _logger.LogInformation("Received interview message (user).");

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

            // Retrieve candidate details for each interview item
            foreach (var interview in interviewMessage.Interviews)
            {
                if (interview.CandidateId.HasValue)
                {
                    var candidate = await userManager.FindByIdAsync(interview.CandidateId.Value.ToString());
                    if (candidate != null)
                    {
                        interview.CandidateName = candidate.UserName;  // Assume UserName holds candidate's full name
                        interview.CandidateEmail = candidate.Email;
                    }
                }
            }

            // Log and send the updated message to the appropriate SignalR group.
            var finalJson = SerializeMessage(interviewMessage);
            _logger.LogInformation("Updated Interview Message: {Message}", finalJson);

            // Using the UserId from the message to target a specific SignalR group.
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

        private string SerializeMessage(InterviewMessage interviewMessage)
        {
            return JsonSerializer.Serialize(interviewMessage);
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
        public string? CandidateName { get; set; }
        public string? CandidateEmail { get; set; }
        public Guid? JobId { get; set; }
    }
}
