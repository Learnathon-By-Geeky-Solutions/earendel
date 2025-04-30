using System;
using System.Text;
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
using TalentMesh.Shared.Authorization;
using TalentMesh.Framework.Infrastructure.Common;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Identity.Users.Services
{
    [ExcludeFromCodeCoverage]
    public class UsersConsumer : RabbitMqConsumerBase
    {
        private readonly IServiceScopeFactory _scopeFactory;

        // Extracted queue names to constants
        private const string ExchangeName = "interview.events.user";
        private const string QueueName = "interview.getCandidate";
        private const string RoutingKey = "interview.getCandidate";

        public UsersConsumer(
            ILogger<UsersConsumer> logger,
            IConnectionFactory connectionFactory,
            IServiceScopeFactory scopeFactory)
            : base(logger, connectionFactory, ExchangeName, QueueName, RoutingKey)
        {
            _scopeFactory = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetUpRabbitMQConnection();
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (sender, ea) => await ProcessMessageAsync(ea);
            _channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }

        private async Task ProcessMessageAsync(BasicDeliverEventArgs ea)
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
            if (interviewMessage.Interviews is not null && interviewMessage.Interviews.Count > 0)
            {
                await PopulateCandidateDetails(interviewMessage.Interviews, userManager);
            }
            else
            {
                LoggerHelper.LogWarning(_logger, "No interviews found in the message.");
            }

            // Log and send the updated message
            var finalJson = JsonHelper.Serialize(interviewMessage);
            LoggerHelper.LogInformation(_logger, "Updated Interview Message", finalJson);

            _channel?.BasicAck(ea.DeliveryTag, multiple: false);
        }

        private static async Task PopulateCandidateDetails(List<InterviewItem> interviews, UserManager<TMUser> userManager)
        {
            var candidateIds = interviews
                .Where(interview => interview.CandidateId.HasValue)
                .Select(interview => interview.CandidateId!.Value.ToString())
                .Distinct()
                .ToList();

            var candidates = (await Task.WhenAll(candidateIds.Select(async id =>
                new { Id = id, User = await userManager.FindByIdAsync(id) })))
                .Where(x => x.User != null)
                .ToDictionary(x => x.Id, x => x.User);

            foreach (var interview in interviews)
            {
                if (interview.CandidateId.HasValue && candidates.TryGetValue(interview.CandidateId.Value.ToString(), out var candidate))
                {
                    interview.CandidateName = candidate?.UserName ?? string.Empty;
                    interview.CandidateEmail = candidate?.Email ?? string.Empty;

                }
            }
        }



    }

    [ExcludeFromCodeCoverage]
    public static class LoggerHelper
    {
        public static void LogReceivedMessage(ILogger logger, string message)
        {
            logger.LogInformation("Received {Message}.", message);
        }

        [ExcludeFromCodeCoverage]
        public static void LogInformation(ILogger logger, string message, string data)
        {
            logger.LogInformation("{Message}: {Data}", message, data);
        }

        [ExcludeFromCodeCoverage]
        public static void LogWarning(ILogger logger, string message)
        {
            logger.LogWarning("Warning: {Message}", message);
        }
    }

    [ExcludeFromCodeCoverage]
    public class InterviewMessage
    {
        public Guid InterviewerId { get; set; }
        public Guid UserId { get; set; }
        public List<InterviewItem> Interviews { get; set; } = new();
    }

    [ExcludeFromCodeCoverage]
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
