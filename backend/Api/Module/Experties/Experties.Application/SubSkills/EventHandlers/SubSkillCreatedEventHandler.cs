﻿using TalentMesh.Module.Experties.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.SubSkills.EventHandlers;

[ExcludeFromCodeCoverage]
public class SubSkillCreatedEventHandler(ILogger<SubSkillCreatedEventHandler> logger) : INotificationHandler<SubSkillCreated>
{
    public async Task Handle(SubSkillCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling Sub-Skill created domain event..");
        await Task.FromResult(notification);
        logger.LogInformation("finished handling Sub-Skill created domain event..");
    }
}
