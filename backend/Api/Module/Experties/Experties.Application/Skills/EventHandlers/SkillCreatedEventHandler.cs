using TalentMesh.Module.Experties.Domain.Events;
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
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.Skills.EventHandlers
{
    [ExcludeFromCodeCoverage]
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
            if (notification.Skill is null)
            {
                _logger.LogWarning("SkillCreated event received without a valid Skill entity.");
                return;
            }

            var skillId = notification.Skill.Id;
            var skillName = notification.Skill.Name;

            // Combined user context logging
            ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
            _logger.LogInformation("Handling SkillCreated event for Skill ID: {SkillId} (User: {UserId}, Email: {Email})",
                skillId,
                user?.GetUserId(),
                user?.GetEmail());

            var skillMessage = new
            {
                SkillId = skillId,
                Name = skillName,
                notification.Skill.Description,
            };

            await _messageBus.PublishAsync(skillMessage, "skill.events", "skill.created", cancellationToken);
            await _hubContext.Clients.Group("admin").SendAsync("ReceiveMessage", skillId, skillName);

            _logger.LogInformation("Published SkillCreated message for {SkillName} (ID: {SkillId})", skillName, skillId);
        }
    }
}

