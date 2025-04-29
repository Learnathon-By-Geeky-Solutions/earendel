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
            var skill = notification.Skill!;
            var skillId = skill.Id;
            var skillName = skill.Name;

            // Combined user context logging
            ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
            
            var skillMessage = new
            {
                SkillId = skillId,
                Name = skillName,
                skill.Description
            };

            await _messageBus.PublishAsync(skillMessage, "skill.events", "skill.created", cancellationToken);
            await _hubContext.Clients.Group("admin").SendAsync("ReceiveMessage", skillId, skillName, cancellationToken);

            _logger.LogInformation("Published SkillCreated message for {SkillName} (ID: {SkillId})", skillName, skillId);
        }
    }
}

