using System;
using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Notifications.Application.Notifications.Create.v1;

public sealed record CreateNotificationCommand(
    Guid UserId,
    string Entity,
    [property: DefaultValue(null)] string? EntityType = null,
    [property: DefaultValue(null)] string? Message = null
) : IRequest<CreateNotificationResponse>;
