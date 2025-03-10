﻿using TalentMesh.Module.Experties.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using TalentMesh.Framework.Infrastructure.Messaging;
using TalentMesh.Module.Experties.Domain;
using Microsoft.AspNetCore.SignalR;
using TalentMesh.Framework.Infrastructure.SignalR;
using TalentMesh.Module.Experties.Application.SubSkills.EventHandlers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TalentMesh.Shared.Authorization;

namespace TalentMesh.Module.Experties.Application.Skills.EventHandlers
{
    public class SkillCreatedEventHandler : INotificationHandler<SkillCreated>
    {
        private readonly ILogger<SkillCreatedEventHandler> _logger;
        private readonly IMessageBus _messageBus;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SkillCreatedEventHandler(ILogger<SkillCreatedEventHandler> logger, IMessageBus messageBus, IHubContext<NotificationHub> hubContext, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _messageBus = messageBus;
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Handle(SkillCreated notification, CancellationToken cancellationToken)
        {
            if (notification.Skill is not null)
            {
                _logger.LogInformation("Handling SkillCreated event for Skill ID: {SkillId}", notification.Skill.Id);

                // Prepare a message object with details from the job.
                var skillMessage = new
                {
                    SkillId = notification.Skill.Id,
                    Name = notification.Skill.Name,
                    Description = notification.Skill.Description,
                };

                ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
                string? email = user?.GetEmail();
                string? userId = user?.GetUserId();

                _logger.LogInformation("email: {Email}", email);
                _logger.LogInformation("userId: {UserId}", userId);


                // Publish the message to a RabbitMQ exchange.
                // Example: Exchange "job.events", Routing Key "job.created"
                await _messageBus.PublishAsync(skillMessage, exchange: "skill.events", routingKey: "skill.created", cancellationToken);
                await _hubContext.Clients.Group("admin").SendAsync("ReceiveMessage", notification.Skill.Id, notification.Skill.Name);

                _logger.LogInformation("Published SkillCreated message for Skill ID: {SkillId}", notification.Skill.Id);
            }
            else
            {
                _logger.LogWarning("JobCreated event received without a valid Job entity.");
            }
        }
    }
}

