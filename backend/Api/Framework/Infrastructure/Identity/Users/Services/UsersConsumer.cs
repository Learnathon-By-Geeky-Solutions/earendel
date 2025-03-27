using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
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

        // Extracted queue names to constants
        private const string ExchangeName = "interview.events.user";
        private const string QueueName = "interview.getCandidate";
        private const string RoutingKey = "interview.getCandidate";

        public UsersConsumer(
            ILogger<UsersConsumer> logger,
            IConnectionFactory connectionFactory,
            IHubContext<NotificationHub> hubContext,
            IServiceScopeFactory scopeFactory)
            : base(logger, connectionFactory, ExchangeName, QueueName, RoutingKey)
        {
            _hubContext = hubContext;
            _scopeFactory = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetUpRabbitMQConnection();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (sender, ea) => await ProcessMessageAsync(ea, stoppingToken);

            _channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }

        private async Task ProcessMessageAsync(BasicDeliverEventArgs ea, CancellationToken stoppingToken)
        {
            LoggerHelper.LogReceivedMessage(_logger, "interview message (user)");

            var messageJson = Encoding.UTF8.GetString(ea.Body.ToArray());
            var interviewMessage = JsonHelper.Deserialize<InterviewMessage>(messageJson);

            if (interviewMessage == null)
            {
                LoggerHelper.LogWarning(_logger, "Message deserialization failed.");
                return;
            }

            using var scope = _scopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<TMUser>>();

            // Batch-fetch candidate details to avoid repeated DB calls
            await PopulateCandidateDetails(interviewMessage.Interviews, userManager);

            // Log and send the updated message
            var finalJson = JsonHelper.Serialize(interviewMessage);
            LoggerHelper.LogInformation(_logger, "Updated Interview Message", finalJson);

            await _hubContext.Clients.Group($"user:{interviewMessage.UserId}").SendAsync("ReceiveMessage", finalJson);

            _channel.BasicAck(ea.DeliveryTag, multiple: false);
        }

        private async Task PopulateCandidateDetails(List<InterviewItem> interviews, UserManager<TMUser> userManager)
        {
            var candidateIds = new HashSet<string>();

            foreach (var interview in interviews)
            {
                if (interview.CandidateId.HasValue)
                {
                    candidateIds.Add(interview.CandidateId.Value.ToString());
                }
            }

            var candidates = new Dictionary<string, TMUser>();
            foreach (var id in candidateIds)
            {
                var candidate = await userManager.FindByIdAsync(id);
                if (candidate != null)
                {
                    candidates[id] = candidate;
                }
            }

            foreach (var interview in interviews)
            {
                if (interview.CandidateId.HasValue && candidates.ContainsKey(interview.CandidateId.Value.ToString()))
                {
                    var candidate = candidates[interview.CandidateId.Value.ToString()];
                    interview.CandidateName = candidate.UserName;
                    interview.CandidateEmail = candidate.Email;
                }
            }
        }
    }

    public static class JsonHelper
    {
        private static readonly JsonSerializerOptions Options = new() { PropertyNameCaseInsensitive = true };

        public static string Serialize<T>(T obj) => JsonSerializer.Serialize(obj, Options);

        public static T? Deserialize<T>(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(json, Options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing message: {ex.Message}");
                return default;
            }
        }
    }

    public static class LoggerHelper
    {
        public static void LogReceivedMessage(ILogger logger, string message)
        {
            logger.LogInformation($"Received {message}.");
        }

        public static void LogInformation(ILogger logger, string message, string data)
        {
            logger.LogInformation($"{message}: {data}");
        }

        public static void LogWarning(ILogger logger, string message)
        {
            logger.LogWarning(message);
        }
    }

    public class InterviewMessage
    {
        public Guid InterviewerId { get; set; }
        public Guid UserId { get; set; }
        public List<InterviewItem> Interviews { get; set; } = new();
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
